using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _levelMenu;
        [SerializeField] private GameObject _settingsMenu;
        
        public void Continue()
        {
            CoreSceneController.Instance.ContinueGame();
        }

        public void ShowLevelMenu()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _levelMenu.SetActive(true);
        }

        public void ShowMainMenu()
        {
            _mainMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _levelMenu.SetActive(false);
        }
        
        public void ShowSettingsMenu()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(true);
            _levelMenu.SetActive(false);
        }

        public void LoadLevel(int index)
        {
            CoreSceneController.Instance.LoadLevel(index);
        }
        
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}