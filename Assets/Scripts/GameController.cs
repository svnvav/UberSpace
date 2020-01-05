
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

        [SerializeField] private PlanetFactory _planetFactory;
        [SerializeField] private PersistentStorage _storage;

        [NonSerialized] private int _loadedLevelBuildIndex;
        
        private List<Planet> _planets;

        private void Awake()
        {
            _planets = new List<Planet>();
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

            StartCoroutine(LoadLevel(1));
        }

        private void Update()
        {
            GameLevel.Current.GameUpdate();
        }

        public void AddPlanet(Planet planet)
        {
            
        }
        
        private IEnumerator LoadLevel(int levelBuildIndex)
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
        
        public override void Save(GameDataWriter writer)
        {
            
        }

        public override void Load(GameDataReader reader)
        {
            
        }
        
        
    }
}
