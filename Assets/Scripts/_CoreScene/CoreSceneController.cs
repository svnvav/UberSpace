using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        
        private const string MainMenuSceneName = "MainMenu";
        private const string GameSceneName = "Game";
        private const string LevelScenePrefix = "Level";
        private const string StartLevelPostfix = "1";

        [SerializeField] private Text _loadingProgressText;
        private Scene _mainMenuScene, _gameScene, _levelScene;
        private AsyncOperation _loadingOp;
        private string _levelPostfix;

        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator Start()
        {
            yield return ResolveStartScenes();
            _loadingProgressText.gameObject.SetActive(false);
        }

        private void Update()
        {
        }

        public void ContinueGame()
        {
            
        }

        public void LoadLevel(string postfix)
        {
            StartCoroutine(LoadLevelRoutine(postfix));
        }

        private IEnumerator LoadLevelRoutine(string postfix)
        {
            _loadingProgressText.gameObject.SetActive(true);
            yield return LoadLevelScene($"{LevelScenePrefix}{postfix}");
            yield return ResolveStartScenes();
            _loadingProgressText.gameObject.SetActive(false);
        }

        public void GoToMainMenu()
        {
        }

        private IEnumerator ResolveStartScenes()
        {
            FindLevelScene();
            if (_levelScene.IsValid())
            {
                yield return LoadGameScene();
                GameController.Instance.Play();
                yield break;
            }
    
            _gameScene = SceneManager.GetSceneByName(GameSceneName);
            if (_gameScene.IsValid())
            {
                yield return LoadLevelScene($"{LevelScenePrefix}{StartLevelPostfix}");
                GameController.Instance.Play();
                yield break;
            }

            _mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
            if (!_mainMenuScene.IsValid())
            {
                yield return LoadMainMenuScene();
            }
        }
        
        private IEnumerator LoadLevelScene(string levelSceneName)
        {
            yield return UnloadMainMenuScene();
            
            //TODO: refactor that part with another loading routines to reduce duplication 
            _loadingOp = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);
            yield return _loadingOp;
            _levelScene = SceneManager.GetSceneByName(levelSceneName);
        }
        
        private IEnumerator LoadGameScene()
        {
            yield return UnloadMainMenuScene();
            
            _loadingOp = SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Additive);
            yield return _loadingOp;
            _gameScene = SceneManager.GetSceneByName(GameSceneName);
        }

        private IEnumerator LoadMainMenuScene()
        {
            if (GameController.Instance != null) GameController.Instance.Stop();

            yield return UnloadGameScene();
            yield return UnloadLevelScene();
            
            _loadingOp = SceneManager.LoadSceneAsync(MainMenuSceneName, LoadSceneMode.Additive);
            yield return _loadingOp;
            _mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
        }

        private IEnumerator UnloadMainMenuScene()
        {
            _mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
            if(!_mainMenuScene.IsValid()) yield break;

            _loadingOp = SceneManager.UnloadSceneAsync(_mainMenuScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);//TODO: test UnloadSceneOptions
            yield return _loadingOp;

            _loadingOp = Resources.UnloadUnusedAssets();
            yield return _loadingOp;
        }

        //TODO: reduce duplication with UnloadMainMenuScene by ref parameter
        private IEnumerator UnloadGameScene()
        {
            _gameScene = SceneManager.GetSceneByName(GameSceneName);
            if(!_gameScene.IsValid()) yield break;

            _loadingOp = SceneManager.UnloadSceneAsync(_gameScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            yield return _loadingOp;

            _loadingOp = Resources.UnloadUnusedAssets();
            yield return _loadingOp;
        }

        private IEnumerator UnloadLevelScene()
        {
            FindLevelScene();
            if(!_levelScene.IsValid()) yield break;
            
            _loadingOp = SceneManager.UnloadSceneAsync(_levelScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            yield return _loadingOp;

            _loadingOp = Resources.UnloadUnusedAssets();
            yield return _loadingOp;
        }

        private void FindLevelScene()
        {
            if (_levelScene.IsValid()) return;
            
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    _levelScene = scene;
                }
            }
        }
    }
}