using UnityCarouselUI;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class LevelSelectMenu : MonoBehaviour
    {
        [SerializeField] private LevelsInfoSO _levelsInfo;
        [SerializeField] private GameObject _levelPagePrefab;
        [SerializeField] private GameObject _indicatorPrefab;
        [SerializeField] private CarouselSwiper _carousel;

        private LevelPage[] _levelPages;

        private void Start()
        {
            _levelPages = new LevelPage[_levelsInfo.Levels.Length];
            for (var i = 0; i < _levelsInfo.Levels.Length; i++)
            {
                var levelInfo = _levelsInfo.Levels[i];

                var pageInstance = Instantiate(_levelPagePrefab);
                _carousel.AddToPages(pageInstance.transform);
                var indicator = Instantiate(_indicatorPrefab);
                _carousel.AddToIndicators(indicator.transform);
                indicator.GetComponent<CarouselIndicator>().SetDotColor(levelInfo.Color);

                var levelPage = pageInstance.GetComponent<LevelPage>();
                levelPage.Initialize(levelInfo);
                _levelPages[i] = levelPage;
            }

            _carousel.Initialize();
            UpdateLevels();
        }

        private void UpdateLevels()
        {
            foreach (var levelPage in _levelPages)
            {
                levelPage.UpdateUserData();
            }
        }
    }
}