using System.IO;
using System.Linq;
using Catlike.ObjectManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelPage : MonoBehaviour
    {
        [SerializeField] private Transform _stagesContainer;
        [SerializeField] private GameObject _levelStagePrefab;
        [SerializeField] private Image _preview;
        [SerializeField] private LevelPageStarsContainer _starsContainer;

        private LevelInfo _levelInfo;

        public void Initialize(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;
            _preview.sprite = _levelInfo.Preview;
        }

        public void UpdateUserData()
        {
            UpdateStages();
            UpdateStars();
        }

        private void UpdateStages()
        {
            var fileNames = Directory.EnumerateFiles(PersistentStorage.Instance.SaveFolderPath)
                .Select(filePath => filePath.Substring(PersistentStorage.Instance.SaveFolderPath.Length + 1))
                .Where(fileName => fileName.StartsWith($"{GameController.SaveFileName}_{_levelInfo.Name}"))
                .ToArray();

            foreach (var levelStageMenuItem in _stagesContainer.GetComponentsInChildren<LevelStageMenuItem>())
            {
                DestroyImmediate(levelStageMenuItem.gameObject);
            }

            /*_stagesContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * (fileNames.Length + 1));
            _stagesContainer.SetSiblingIndex(levelIndex + 1);*/
            
            var beginStageGO = Instantiate(_levelStagePrefab, _stagesContainer);
            var levelStage = beginStageGO.GetComponent<LevelStageMenuItem>();
            //levelStage.Initialize(this, $"Begin_{levelIndex}");
            for (int i = 0; i < fileNames.Length; i++)
            {
                var stageGO = Instantiate(_levelStagePrefab, _stagesContainer);
                levelStage = stageGO.GetComponent<LevelStageMenuItem>();
                //levelStage.Initialize(this, fileNames[i]);
            }
        }

        private void UpdateStars()
        {
            
        }
    }
}