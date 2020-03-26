
using System;
using System.Collections;
using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    public class GameController : PersistableObject
    {
        public static GameController Instance { get; private set; }
        
        const int saveVersion = 1;

        [SerializeField] private HUD _hud;
        
        [SerializeField] private TaxiService _taxiService;
        
        [SerializeField] private PersistentStorage _storage;

        [SerializeField] private PlanetFactory _planetFactory;
        public PlanetFactory PlanetFactory => _planetFactory;
        
        [SerializeField] private RaceFactory _raceFactory;
        public RaceFactory RaceFactory => _raceFactory;
        
        private float _gameSpeed = 1f;
        private bool _paused;
        private float _beforePauseGameSpeed;
        private float _beforePauseTimeScale;
        
        
        public float GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = value;
        }

        [NonSerialized] private int _loadedLevelBuildIndex;

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

        private void Start()
        {
#if UNITY_EDITOR
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name.Contains("Level"))
                {
                    SceneManager.SetActiveScene(loadedScene);
                    _loadedLevelBuildIndex = loadedScene.buildIndex;
                    return;
                }
            }
#endif

            StartCoroutine(LoadLevelScene(1));
        }

        private void Update()
        {
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

            foreach (var planet in _planets)
            {
                planet.GameUpdate(deltaTime);
            }

            _taxiService.GameUpdate(deltaTime);
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                _storage.Save(this, saveVersion);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Restart();
                _storage.Load(this);
            }
        }

        public void Pause()
        {
            if (_paused)
            {
                return;
            }

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
            
            _hud.OnUnpause();
            GameSpeed = _beforePauseGameSpeed;
            Time.timeScale = _beforePauseTimeScale;
        }
        
        public void AddPlanet(Planet planet)
        {
            planet.SaveIndex = _planets.Count;
            _planets.Add(planet);
            //planet.OnDie += RemovePlanet;
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

        public void RemoveRace(Race race)
        {
            var index = race.SaveIndex;
            var last = _races.Count - 1;
            if (index < last)
            {
                _races[index] = _races[last];
                _races[index].SaveIndex = index;
            }
            
            _races.RemoveAt(last);
            //TODO: remove race ui
        }

        public void TransferRace(Race race, Planet departure, Planet destination)
        {
            _taxiService.AddOrder(race, departure, destination);
        }

        private IEnumerator LoadLevelScene(int levelBuildIndex)
        {
            enabled = false;
            if (_loadedLevelBuildIndex > 0)
            {
                yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
            }

            yield return
                SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
            _loadedLevelBuildIndex = levelBuildIndex;
            enabled = true;
        }

        public void Restart()
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
        
        public override void Save(GameDataWriter writer)
        {
            writer.Write(_loadedLevelBuildIndex);
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

        public override void Load(GameDataReader reader)
        {
            var version = reader.Version;
            if (version > saveVersion)
            {
                Debug.LogError("Unsupported future save version " + version);
                return;
            }

            StartCoroutine(LoadGame(reader));
        }

        private IEnumerator LoadGame(GameDataReader reader)
        {
            yield return LoadLevelScene(reader.ReadInt());
            
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
