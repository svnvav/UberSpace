using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateController : MonoBehaviour
    {
        internal /*const*/ string MainMenuSceneName = "MainMenu";
        internal /*const*/ string GameSceneName = "Game";
        internal /*const*/ string LevelScenePrefix = "Level";

        [SerializeField] private Text _loadingProgressText;
        
        private int _currentLevelIndex = 0;
        private string _saveFileName = "Begin_0";
        
        private GameStateMachine _stateMachine;
        
        private AsyncOperation _loadingOp;

        public int CurrentLevelIndex => _currentLevelIndex;
        public string SaveFileName => _saveFileName;

        private IEnumerator Start()
        {
            var levelState = new LevelState();
            var menuState = new MenuState();
            var fsmTransitions = new Dictionary<StateTransition, GameState>()
            {
                {new StateTransition(levelState, Command.ToMenu), menuState},
                {new StateTransition(levelState, Command.LoadLast), levelState},
                {new StateTransition(menuState, Command.Play), levelState}
            };
            
            _stateMachine = new GameStateMachine(this, fsmTransitions);
            yield return DefineStartState(levelState, menuState);
        }

        public void GoToLevel(string saveFileName)
        {
            _saveFileName = saveFileName;
            
            var split = _saveFileName.Split('_');
            
            _currentLevelIndex = Int32.Parse(split[1]);

            StartCoroutine(_stateMachine.MoveNext(Command.Play));
        }

        public void LoadLast()
        {
            StartCoroutine(_stateMachine.MoveNext(Command.LoadLast));
        }

        public void GoToMainMenu()
        {
            StartCoroutine(_stateMachine.MoveNext(Command.ToMenu));
        }

        public void ShowHideLoadingScreen(bool showFlag)
        {
            _loadingProgressText.gameObject.SetActive(showFlag);
        }
        
        public void SetProgress(float value)
        {
            _loadingProgressText.text = $"Loading {100 * value} %";
        }

        private IEnumerator DefineStartState(GameState levelState, GameState menuState)
        {
            var loadedLevelSceneIndex = LoadedLevelSceneIndex();
            if (loadedLevelSceneIndex != -1)
            {
                _currentLevelIndex = loadedLevelSceneIndex;
                _saveFileName = $"Begin_{_currentLevelIndex}";
            }
            if (SceneManager.GetSceneByName(GameSceneName).IsValid() || loadedLevelSceneIndex != -1)
            {
                yield return UnloadScene(MainMenuSceneName);
                yield return _stateMachine.Initialize(levelState);
            }
            else
            {
                yield return UnloadScene(GameSceneName);
                yield return UnloadScene(FindFirstLoadedLevelSceneName());
                yield return _stateMachine.Initialize(menuState);
            }
        }

        private int LoadedLevelSceneIndex()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    return Int32.Parse(scene.name.Replace(LevelScenePrefix, ""));
                }
            }

            return -1;
        }
        
        private IEnumerator UnloadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            yield return UnloadScene(scene);
        }

        public IEnumerator UnloadScene(Scene scene)
        {
            if(!scene.IsValid()) yield break;

            _loadingOp = SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            yield return _loadingOp;

            _loadingOp = Resources.UnloadUnusedAssets();
            yield return _loadingOp;
        }
        
        private string FindFirstLoadedLevelSceneName()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    return scene.name;
                }
            }

            return "";
        }
    }
}