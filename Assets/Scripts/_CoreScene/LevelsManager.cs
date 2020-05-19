using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    public class LevelsManager : MonoBehaviour
    {
        [NonSerialized] private int _loadedLevelBuildIndex;

        private AsyncOperation _loading;
        
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

            StartCoroutine(LoadLevelScene(3));
        }
        
        

        public IEnumerator LoadLevelScene(string levelSceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scene = SceneManager.GetSceneByBuildIndex(i);
                if (scene.name == levelSceneName)
                {
                    yield return LoadLevelScene(i);
                    break;
                }
            }
        }
        
        public IEnumerator LoadLevelScene(int levelBuildIndex)
        {
            if (_loadedLevelBuildIndex > 0)
            {
                yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
            }

            _loading = SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
            yield return _loading;
                
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
            _loadedLevelBuildIndex = levelBuildIndex;
        }
        
        
    }
}