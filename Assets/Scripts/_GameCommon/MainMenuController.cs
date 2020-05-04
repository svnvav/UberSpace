using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class MainMenuController : MonoBehaviour
    {
        
        public void Continue()
        {
            CoreSceneController.Instance.ContinueGame();
        }

        public void LoadLevelMenu()
        {
            
        }

        public void LoadLevel(int index)
        {
            CoreSceneController.Instance.StartLevel(index);
        }

        public void Settings()
        {
            
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}