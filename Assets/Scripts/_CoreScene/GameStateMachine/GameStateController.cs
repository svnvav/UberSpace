using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateController : MonoBehaviour
    {
        internal /*const*/ string MainMenuSceneName = "MainMenu";
        internal /*const*/ string GameSceneName = "Game";
        internal /*const*/ string LevelScenePrefix = "Level";

        private int _currentLevelIndex = 1;
        
        private GameStateMachine _stateMachine;
        
        private AsyncOperation _loadingOp;

        public int CurrentLevelIndex => _currentLevelIndex;

        private IEnumerator Start()
        {
            var levelState = new LevelState();
            var menuState = new MenuState();
            var fsmTransitions = new Dictionary<StateTransition, GameState>()
            {
                {new StateTransition(levelState, Command.ToMenu), menuState},
                {new StateTransition(menuState, Command.Play), levelState}
            };
            
            _stateMachine = new GameStateMachine(this, fsmTransitions);
            yield return DefineStartState(levelState, menuState);
        }

        private IEnumerator DefineStartState(GameState levelState, GameState menuState)
        {
            if (SceneManager.GetSceneByName(GameSceneName).IsValid() || IsLevelSceneLoaded())
            {
                yield return UnloadScene(MainMenuSceneName);
                _stateMachine.Initialize(levelState);
            }
            else
            {
                yield return UnloadScene(GameSceneName);
                yield return UnloadScene(FindFirstLoadedLevelSceneName());
                _stateMachine.Initialize(menuState);
            }
        }

        private bool IsLevelSceneLoaded()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    return true;
                }
            }

            return false;
        }
        
        private IEnumerator UnloadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
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