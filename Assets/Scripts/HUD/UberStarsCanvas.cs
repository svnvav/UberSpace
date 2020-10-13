using UnityEngine;

namespace Svnvav.UberSpace
{
    public class UberStarsCanvas : MonoBehaviour
    {
        [SerializeField] private UberStar _starPrefab;
        [SerializeField] private RectTransform _panel;

        private UberStar[] _stars;

        public void RefreshView()
        {
            if (_stars == null || _stars.Length != GameLevel.Current.MaxStarsCount)
            {
                if (_stars != null)
                {
                    foreach (var uberStar in _stars)
                    {
                        Destroy(uberStar.gameObject);
                    }
                }
                _stars = new UberStar[GameLevel.Current.MaxStarsCount];
                for (int i = 0; i < GameLevel.Current.MaxStarsCount; i++)
                {
                    _stars[i] = Instantiate(_starPrefab, _panel);
                }
            }
            
            for (int i = 0; i < GameLevel.Current.CurrentStarsCount; i++)
            {
                _stars[i].SetFilled(true);
            }
            
            for (int i = GameLevel.Current.CurrentStarsCount; i < GameLevel.Current.MaxStarsCount; i++)
            {
                _stars[i].SetFilled(false);
            }
        }
    }
}