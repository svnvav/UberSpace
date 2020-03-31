using UnityEngine;

namespace Svnvav.UberSpace
{
    public class MainMenuController : MonoBehaviour
    {
        public void Continue()
        {
            PersistentGameManager.Instance.ContinueGame();
        }

        public void LoadLevelMenu()
        {
            
        }

        public void LoadLevel(int index)
        {
            PersistentGameManager.Instance.StartLevel(index);
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