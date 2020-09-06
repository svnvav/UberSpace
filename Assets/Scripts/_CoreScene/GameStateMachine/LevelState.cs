﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class LevelState : GameState
    {
        private AsyncOperation _loadingOp;
        private Scene _gameScene;
        private Scene _levelScene;
        
        public IEnumerator Enter(GameStateController controller)
        {
            controller.ShowHideLoadingScreen(true);
            
            var gameSceneName = controller.GameSceneName;
            
            _gameScene = SceneManager.GetSceneByName(gameSceneName);
            if (!_gameScene.IsValid() || !_gameScene.isLoaded)
            {
                _loadingOp = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
                while (!_loadingOp.isDone)
                {
                    yield return null;
                    controller.SetProgress(_loadingOp.progress * 0.5f);
                }

                _gameScene = SceneManager.GetSceneByName(gameSceneName);
            }

            yield return new WaitForSeconds(1f);
            
            var levelSceneName = $"{controller.LevelScenePrefix}{controller.CurrentLevelIndex}";
            
            _levelScene = SceneManager.GetSceneByName(levelSceneName);

            if (!_levelScene.IsValid() || !_levelScene.isLoaded)
            {
                _loadingOp = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);
                while (!_loadingOp.isDone)
                {
                    yield return null;
                    controller.SetProgress(0.5f + _loadingOp.progress * 0.5f);
                }

                _levelScene = SceneManager.GetSceneByName(levelSceneName);
            }

            controller.ShowHideLoadingScreen(false);
        }

        public IEnumerator Exit(GameStateController controller)
        {
            yield return controller.UnloadScene(_levelScene);
            yield return controller.UnloadScene(_gameScene);
        }
    }
}