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
            var gameSceneName = controller.GameSceneName;
            _loadingOp = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
            while (!_loadingOp.isDone)
            {
                yield return null;
            }
            _gameScene = SceneManager.GetSceneByName(gameSceneName);
            
            var levelSceneName = $"{controller.LevelScenePrefix}{controller.CurrentLevelIndex}";
            _loadingOp = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);
            while (!_loadingOp.isDone)
            {
                yield return null;
            }
            _levelScene = SceneManager.GetSceneByName(levelSceneName);
        }

        public IEnumerator Exit(GameStateController controller)
        {
            yield break;
        }
    }
}