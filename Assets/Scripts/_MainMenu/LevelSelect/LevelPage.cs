using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catlike.ObjectManagement;
using UnityCarouselUI;
using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelPage : MonoBehaviour
    {
        [SerializeField] private Transform _stagesContainer;
        [SerializeField] private RectTransform _stagePicker;
        [SerializeField] private float _stagePickerSwipeTime = 0.3f;
        [SerializeField] private GameObject _levelStagePrefab;
        [SerializeField] private Transform _previewContainer;
        [SerializeField] private LevelPageStarsContainer _starsContainer;

        private LevelInfo _levelInfo;
        private int _levelIndex;
        private LevelPreview _levelPreviewInstance;
        
        private List<LevelStageMenuItem> _stages;
        private int _pickedStage;
        private Coroutine _pickerRoutine;

        public LevelStageMenuItem SelectedStage => _stages[_pickedStage];

        public void Initialize(LevelInfo levelInfo, int levelIndex)
        {
            _levelIndex = levelIndex;
            _levelInfo = levelInfo;
            _levelPreviewInstance = Instantiate(levelInfo.Preview, _previewContainer);

            _stages = new List<LevelStageMenuItem>(8);
        }

        public void PassPageRelativePosition(float value)
        {
            _levelPreviewInstance.SetTransparency(1f - value);
        }
        
        public void UpdateUserData()
        {
            UpdateStages();
        }

        public void DestroyOldSavesAfterSelected()
        {
            for (int i = _pickedStage + 1; i < _stages.Count; i++)
            {
                var filePath = Path.Combine(PersistentStorage.Instance.SaveFolderPath, _stages[i].SaveFileName);
                File.Delete(filePath);
            }
        }
        
        private void UpdateStages()
        {
            var fileNames = Directory.EnumerateFiles(PersistentStorage.Instance.SaveFolderPath)
                .Select(filePath => filePath.Substring(PersistentStorage.Instance.SaveFolderPath.Length + 1))
                .Where(fileName => fileName.StartsWith($"{GameController.SaveFileName}_{_levelIndex}"))
                .ToArray();//refactor

            foreach (var levelStageMenuItem in _stages)
            {
                levelStageMenuItem.Dispose();
                DestroyImmediate(levelStageMenuItem.gameObject);
            }
            _stages.Clear();

            var beginStageGO = Instantiate(_levelStagePrefab, _stagesContainer);
            var levelStage = beginStageGO.GetComponent<LevelStageMenuItem>();
            levelStage.Initialize($"Begin_{_levelIndex}", _levelInfo.LevelStageSprites[0], () => PickStage(0));
            _stages.Add(levelStage);
            for (int i = 0; i < fileNames.Length; i++)
            {
                var stageGO = Instantiate(_levelStagePrefab, _stagesContainer);
                levelStage = stageGO.GetComponent<LevelStageMenuItem>();
                var capturedI = i;
                levelStage.Initialize(fileNames[i], _levelInfo.LevelStageSprites[i + 1], () => PickStage(capturedI + 1));
                _stages.Add(levelStage);
            }

            PickStage(0);
        }

        private void UpdateStars()
        {
            var starsCount = _pickedStage == 0 ? 5 : PlayerPrefs.GetInt(_stages[_pickedStage].SaveFileName);
            _starsContainer.SetStars(starsCount);
        }

        private void PickStage(int i)
        {
            _pickedStage = i;
            if(_pickerRoutine != null)
                StopCoroutine(_pickerRoutine);
            _pickerRoutine = StartCoroutine(PickerMove(_stagePickerSwipeTime));
            UpdateStars();
        }

        private IEnumerator PickerMove(float seconds)
        {
            var startX = _stagePicker.anchoredPosition.x;
            var finishX = _pickedStage * 128f;
            var position = new Vector2(0f, _stagePicker.anchoredPosition.y);
            var t = 0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime / seconds;
                position.x = Mathf.Lerp(startX, finishX, t);
                _stagePicker.anchoredPosition = position;
                yield return null;
            }
        }
    }
}