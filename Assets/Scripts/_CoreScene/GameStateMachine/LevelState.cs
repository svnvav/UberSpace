using System.Collections;
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

            yield return new WaitForSeconds(1f);//For Debug
            
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

            if (!controller.SaveFileName.StartsWith("Begin"))
            {
                GameController.Instance.LoadFromSaves(controller.SaveFileName);
            }
            
            controller.ShowHideLoadingScreen(false);
            
            GameController.Instance.Play();
        }

        public IEnumerator Exit(GameStateController controller)
        {
            GameController.Instance.Stop();
            yield return controller.UnloadScene(_levelScene);
            yield return controller.UnloadScene(_gameScene);
        }
    }
}