using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateController : MonoBehaviour
    {
        public string SaveFileName => _saveFileName;
        public string MainMenuSceneName => _mainMenuSceneName;
        public string GameSceneName => _gameSceneName;
        public LevelInfo CurrentLevel => _levelsInfo.Levels[_currentLevelIndex];
        public int CurrentLevelIndex => _currentLevelIndex;


        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private LevelsInfoSO _levelsInfo;

        [SerializeField] private LoadingScreen _loadingScreen;
        
        private int _currentLevelIndex = 0;
        private string _saveFileName = "Begin_0";
        
        private GameStateMachine _stateMachine;
        
        private AsyncOperation _loadingOp;

        private IEnumerator Start()
        {
            var levelState = new LevelState();
            var menuState = new MenuState();
            var fsmTransitions = new Dictionary<StateTransition, IGameState>()
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

        public void LoadLast(string saveFileName)
        {
            _saveFileName = saveFileName;
            
            var split = _saveFileName.Split('_');
            
            _currentLevelIndex = Int32.Parse(split[1]);
            
            StartCoroutine(_stateMachine.MoveNext(Command.LoadLast));
        }

        public void GoToMainMenu()
        {
            StartCoroutine(_stateMachine.MoveNext(Command.ToMenu));
        }

        public IEnumerator StartLoading()
        {
            return _loadingScreen.ShowRoutine();
        }

        public IEnumerator FinishLoading()
        {
            return _loadingScreen.HideRoutine();
        }
        
        public void SetProgress(float value)
        {
            _loadingScreen.SetProgress(value);
        }

        private IEnumerator DefineStartState(IGameState levelState, IGameState menuState)
        {
            var loadedLevelIndex = LoadedLevelSceneIndex();
            if (loadedLevelIndex != -1)
            {
                _currentLevelIndex = loadedLevelIndex;
                _saveFileName = $"Begin_{_currentLevelIndex}";
            }
            if (SceneManager.GetSceneByName(_gameSceneName).IsValid() || loadedLevelIndex != -1)
            {
                yield return UnloadScene(_mainMenuSceneName);
                yield return _stateMachine.Initialize(levelState);
            }
            else
            {
                yield return UnloadScene(_gameSceneName);
                if(_currentLevelIndex != -1)
                    yield return UnloadScene(_levelsInfo.Levels[_currentLevelIndex].SceneName);
                yield return _stateMachine.Initialize(menuState);
            }
        }

        private int LoadedLevelSceneIndex()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var index = Array.FindIndex(_levelsInfo.Levels, info => info.SceneName == scene.name);
                
                if (index != -1) return index;
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

            _loadingOp = Resources.UnloadUnusedAssets();
            yield return _loadingOp;
        }
    }
}