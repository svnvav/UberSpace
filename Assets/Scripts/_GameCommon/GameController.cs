using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameController : MonoBehaviour, IPersistable
    {
        public static GameController Instance { get; private set; }
        
        public const int SaveVersion = 1;
        public const string SaveFileName = "GameLevelSave";

        [SerializeField] private HUD _hud;
        
        [SerializeField] private TaxiService _taxiService;

        [SerializeField] private PlanetFactory _planetFactory;
        public PlanetFactory PlanetFactory => _planetFactory;
        
        [SerializeField] private RaceFactory _raceFactory;
        public RaceFactory RaceFactory => _raceFactory;

        private bool _started;
        private bool _paused;
        private float _beforePauseGameSpeed;
        private float _beforePauseTimeScale;
        private float _gameSpeed = 1f;
        public float GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = value;
        }

        private ActionContainer<Planet> _onAddPlanet, _onRemovePlanet;

        public ActionContainer<Planet> OnAddPlanet => _onAddPlanet;
        public ActionContainer<Planet> OnRemovePlanet => _onRemovePlanet;


        private List<Planet> _planets;
        public List<Planet> Planets => _planets;

        private List<Race> _races;
        public List<Race> Races => _races;

        private void Awake()
        {
            Instance = this;
            
            _planets = new List<Planet>();
            _races = new List<Race>();
            
            _onAddPlanet = new ActionContainer<Planet>();
            _onRemovePlanet = new ActionContainer<Planet>();
        }

        private void Update()
        {
            if(!_started) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                var touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null)
                {
                    var blackHole = hit.collider.GetComponent<BlackHole>();
                    if (blackHole != null)
                    {
                        Pause();
                    }
                }
            }

            var deltaTime = _gameSpeed * Time.deltaTime;
            
            GameLevel.Current.GameUpdate(deltaTime);
            if (GameLevel.Current.CurrentStarsCount <= 0)
            {
                GameOver();
            }

            foreach (var planet in _planets)
            {
                planet.GameUpdate(deltaTime);
            }

            _taxiService.GameUpdate(deltaTime);
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                PersistentStorage.Instance.Save(this, SaveVersion, "GameSave");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Flush();
                LoadFromSaves("GameSave");
            }
        }

        public void LoadFromSaves(string safeFileName)
        {
            PersistentStorage.Instance.Load(this, safeFileName);
        }
        
        public void Play()
        {
            _started = true;
            Time.timeScale = 1f;
            _hud.RefreshStarsView();
        }
        
        public void Stop()
        {
            Unpause();//To restore game speed
            Flush();
            _started = false;
        }
        
        public void Pause()
        {
            if (_paused)
            {
                return;
            }

            _paused = true;
            _hud.OnPause();
            _beforePauseGameSpeed = GameSpeed;
            _beforePauseTimeScale = Time.timeScale;
            Time.timeScale = 0.5f;
            GameSpeed = 0.0f;
        }

        public void Unpause()
        {
            if (!_paused)
            {
                return;
            }
            
            _paused = false;
            _hud.OnUnpause();
            GameSpeed = _beforePauseGameSpeed;
            Time.timeScale = _beforePauseTimeScale;
        }

        public void GameOver()
        {
            Time.timeScale = 0.3f;
            GameSpeed = 0.1f;
            _hud.OnGameOver();
        }
        
        public void AddPlanet(Planet planet)
        {
            planet.SaveIndex = _planets.Count;
            _planets.Add(planet);
            _onAddPlanet.Execute(planet);
        }

        public void RemovePlanet(Planet planet)
        {
            var index = planet.SaveIndex;
            var last = _planets.Count - 1;
            if (index < last)
            {
                _planets[index] = _planets[last];
                _planets[index].SaveIndex = index;
            }
            
            _planets.RemoveAt(last);
            _onRemovePlanet.Execute(planet);
        }

        public void AddRace(Race race)
        {
            race.SaveIndex = _races.Count;
            _races.Add(race);
            _hud.AddRaceInfo(race);
        }

        public void OnRaceDead(Race race)
        {
            Debug.Log($"{race.Name} dead");
            RemoveRace(race);
            GameLevel.Current.DecreaseUberStar();
            _hud.RefreshStarsView();
            //TODO:
        }

        public void OnRaceSurvived(Race race)
        {
            Debug.Log($"{race.Name} survived");
            RemoveRace(race);
        }

        private void RemoveRace(Race race)
        {
            var index = race.SaveIndex;
            var last = _races.Count - 1;
            if (index < last)
            {
                _races[index] = _races[last];
                _races[index].SaveIndex = index;
            }
            
            _races.RemoveAt(last);
            _hud.RemoveRaceInfo(race);
        }

        public void TransferRace(Race race, Planet departure, Planet destination)
        {
            _taxiService.AddOrder(race, departure, destination);
        }

        public void Flush()
        {
            foreach (var planet in _planets)
            {
                planet.Recycle();
            }
            _planets.Clear();

            foreach (var race in _races)
            {
                race.Recycle();
            }
            _races.Clear();
        }
        
        public void Save(GameDataWriter writer)
        {
            GameLevel.Current.Save(writer);
            
            writer.Write(_planets.Count);
            foreach (var planet in _planets)
            {
                writer.Write(planet.PrefabId);
                planet.Save(writer);
            }
            
            writer.Write(_races.Count);
            foreach (var race in _races)
            {
                writer.Write(race.PrefabId);
                race.Save(writer);
            }
            
            _taxiService.Save(writer);
        }

        public void Load(GameDataReader reader)
        {
            var version = reader.Version;
            if (version > SaveVersion)
            {
                Debug.LogError("Unsupported future save version " + version);
                return;
            }

            LoadGame(reader);
        }

        private void LoadGame(GameDataReader reader)
        {
            GameLevel.Current.Load(reader);
            
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var prefabId = reader.ReadInt();
                var planet = _planetFactory.Get(prefabId);
                planet.Load(reader);
            }
            
            count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var prefabId = reader.ReadInt();
                var race = _raceFactory.Get(prefabId);
                race.Load(reader);
                if (race.PlanetSaveIndex >= 0)
                {
                    _planets[race.PlanetSaveIndex].AddRace(race, true);
                }
            }
            
            _taxiService.Load(reader);
        }
    }
}
