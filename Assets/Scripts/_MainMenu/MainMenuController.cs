using System.IO;
using System.Linq;
using Catlike.ObjectManagement;
using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private RectTransform _levelStagesContainer;
        [SerializeField] private GameObject _levelStagePrefab;
        
        [SerializeField] private GameObject _continueButton;
        
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _levelMenu;
        [SerializeField] private GameObject _settingsMenu;

        private void OnEnable()
        {
            _continueButton.SetActive(CoreSceneController.Instance.CoreDataProvider.LastLoadedLevelPostfix != 0);
        }

        public void Continue()
        {
            CoreSceneController.Instance.ContinueGame();
        }

        public void ShowLevelMenu()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _levelMenu.SetActive(true);
            ShowLevelStages(1);
        }

        public void ShowLevelStages(int levelIndex)
        {
            var fileNames = Directory.EnumerateFiles(PersistentStorage.Instance.SaveFolderPath)
                .Select(filePath => filePath.Substring(PersistentStorage.Instance.SaveFolderPath.Length + 1))
                .Where(fileName => fileName.StartsWith($"{GameController.SaveFileName}_{levelIndex}"))
                .ToArray();

            foreach (var levelStageMenuItem in _levelStagesContainer.GetComponentsInChildren<LevelStageMenuItem>())
            {
                DestroyImmediate(levelStageMenuItem);
            }
            
            _levelStagesContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * fileNames.Length);

            for (int i = 0; i < fileNames.Length; i++)
            {
                var stageGO = Instantiate(_levelStagePrefab, _levelStagesContainer);
                var levelStage = stageGO.GetComponent<LevelStageMenuItem>();
                levelStage.Initialize(this, fileNames[i]);
            }

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

        public void LoadLevel(string saveFileName)
        {
            CoreSceneController.Instance.LoadLevel(saveFileName);
        }
        
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}