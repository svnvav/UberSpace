
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
        [SerializeField] private PersistentStorage _storage;

        [SerializeField] private PlanetFactory _planetFactory;
        [SerializeField] private RaceFactory _raceFactory;

        [NonSerialized] private int _loadedLevelBuildIndex;
        
        private List<Planet> _planets;
        private List<Race> _races;

        public List<Planet> Planets => _planets;

        private void Awake()
        {
            _planets = new List<Planet>();
            _races = new List<Race>();
            Instance = this;
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
            GameLevel.Current.GameUpdate();

            foreach (var planet in _planets)
            {
                planet.GameUpdate();
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
        }
        
        public void AddRace(Race race)
        {
            _races.Add(race);
        }

        public void RemoveRace(Race race)
        {
            _races.Remove(race);
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
        }
        
        public override void Save(GameDataWriter writer)
        {
            writer.Write(_loadedLevelBuildIndex);
            GameLevel.Current.Save(writer);
            
            writer.Write(_planets.Count);
            foreach (var planet in _planets)
            {
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
                var planet = _planetFactory.Get();
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
