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
            _menuScene = SceneManager.GetSceneByName(menuSceneName);
            if(_menuScene.IsValid() && _menuScene.isLoaded) yield break;
            
            controller.ShowHideLoadingScreen(true);

            _loadingOp = SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
            while (!_loadingOp.isDone)
            {
                yield return null;
                controller.SetProgress(_loadingOp.progress);
            }
            _menuScene = SceneManager.GetSceneByName(menuSceneName);
            
            yield return new WaitForSeconds(1f);//For Debug
            
            controller.ShowHideLoadingScreen(false);
        }

        public IEnumerator Exit(GameStateController controller)
        {
            yield return controller.UnloadScene(_menuScene);
        }
    }
}