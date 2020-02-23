
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
        
        [SerializeField] private Taxi _taxi;
        
        [SerializeField] private PersistentStorage _storage;

        [SerializeField] private PlanetFactory _planetFactory;
        public PlanetFactory PlanetFactory => _planetFactory;
        
        [SerializeField] private RaceFactory _raceFactory;
        public RaceFactory RaceFactory => _raceFactory;
        
        private float _gameSpeed = 1f;
        public float GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = value;
        }

        [NonSerialized] private int _loadedLevelBuildIndex;

        #region Callbacks

        private List<Action<Planet>> _onAddPlanet, _onRemovePlanet;
        public void RegisterOnAddPlanet(Action<Planet> onAddPlanet)
        {
            _onAddPlanet.Add(onAddPlanet);
        }
        
        public void UnregisterOnAddPlanet(Action<Planet> onAddPlanet)
        {
            _onAddPlanet.Remove(onAddPlanet);
        }

        private void OnAddPlanet(Planet planet)
        {
            foreach (var action in _onAddPlanet)
            {
                action(planet);
            }
        }
        
        public void RegisterOnRemovePlanet(Action<Planet> onAddPlanet)
        {
            _onRemovePlanet.Add(onAddPlanet);
        }
        
        public void UnregisterOnRemovePlanet(Action<Planet> onAddPlanet)
        {
            _onRemovePlanet.Remove(onAddPlanet);
        }
        
        private void OnRemovePlanet(Planet planet)
        {
            foreach (var action in _onRemovePlanet)
            {
                action(planet);
            }
        }

        #endregion

        private List<Planet> _planets;
        public List<Planet> Planets => _planets;

        private List<Race> _races;
        

        private void Awake()
        {
            _planets = new List<Planet>();
            _races = new List<Race>();
            Instance = this;
            _onAddPlanet = new List<Action<Planet>>();
            _onRemovePlanet = new List<Action<Planet>>();
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
            var deltaTime = _gameSpeed * Time.deltaTime;
            GameLevel.Current.GameUpdate(deltaTime);

            foreach (var planet in _planets)
            {
                planet.GameUpdate(deltaTime);
            }
            
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

        public void AddPlanet(Planet planet)
        {
            planet.SaveIndex = _planets.Count;
            _planets.Add(planet);
            OnAddPlanet(planet);
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
            OnRemovePlanet(planet);
        }

        public void AddRace(Race race)
        {
            _races.Add(race);
            //TODO: initialize race ui
        }

        public void RemoveRace(Race race)
        {
            _races.Remove(race);
            //TODO: remove race ui
        }

        public void TransferRace(Race race, Planet departure, Planet destination)
        {
            _taxi.AddOrder(race, departure, destination);
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
                _planets[race.PlanetSaveIndex].AddRace(race);
            }
            
        }
    }
}
