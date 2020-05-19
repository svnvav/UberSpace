using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        
        private const string MainMenuSceneName = "MainMenu";
        private const string GameSceneName = "Game";
        private const string LevelScenePrefix = "Level";
        private const string StartLevelPostfix = "1";

        //[SerializeField] private SceneSwitcher _scenesSwitcher;
        
        private Scene _mainMenuScene, _gameScene, _levelScene;
        private AsyncOperation _loadingOp;
        private string _levelPostfix;

        private void Awake()
        {
            Instance = this;
            ResolveStartScenes();
        }

        private void Update()
        {
        }

        public void ContinueGame()
        {
            
        }

        public void LoadLevel(string postfix)
        {
            StartCoroutine(LoadLevelScene($"{LevelScenePrefix}{postfix}"));
        }

        public void GoToMainMenu()
        {
        }

        private void ResolveStartScenes()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    StartCoroutine(LoadGameScene());
                    return;
                }
            }

            _gameScene = SceneManager.GetSceneByName(GameSceneName);
            if (_gameScene.IsValid())
            {
                StartCoroutine(LoadLevelScene($"{LevelScenePrefix}{StartLevelPostfix}"));
                //TODO: Game start (sync with GameController)
            }
        }

        private IEnumerator LoadLevelScene(string levelSceneName)
        {
            yield return UnloadMainMenuScene();
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

        private IEnumerator UnloadMainMenuScene()
        {
            _mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
            if(!_mainMenuScene.IsValid()) yield break;

            _loadingOp = SceneManager.UnloadSceneAsync(_mainMenuScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);//TODO: test UnloadSceneOptions

            yield return _loadingOp;

            _loadingOp = Resources.UnloadUnusedAssets();

            yield return _loadingOp;
        }
        
        private enum GameState
        {
            Menu,
            Loading,
            Game
        }
    }
}