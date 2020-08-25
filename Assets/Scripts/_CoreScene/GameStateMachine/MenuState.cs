using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class MenuState : GameState
    {
        private AsyncOperation _loadingOp;
        private Scene _menuScene;
        
        public IEnumerator Enter(GameStateController controller)
        {
            var menuSceneName = controller.MainMenuSceneName;
            _loadingOp = SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
            while (!_loadingOp.isDone)
            {
                yield return null;
            }
            _menuScene = SceneManager.GetSceneByName(menuSceneName);
        }

        public IEnumerator Exit(GameStateController controller)
        {
            yield break;
        }
    }
}