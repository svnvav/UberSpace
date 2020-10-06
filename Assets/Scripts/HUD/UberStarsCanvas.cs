using UnityEngine;

namespace Svnvav.UberSpace
{
    public class UberStarsCanvas : MonoBehaviour
    {
        [SerializeField] private UberStar _starPrefab;
        [SerializeField] private int _count;
        [SerializeField] private int _currentScore;
        [SerializeField] private RectTransform _panel;

        private UberStar[] _stars;

        private void Awake()
        {
            _stars = new UberStar[_count];
            for (int i = 0; i < _count; i++)
            {
                _stars[i] = Instantiate(_starPrefab, _panel);
            }
            RefreshView();
        }

        public void Decrease()
        {
            if(_currentScore == 0) return;
            
            _currentScore--;
            RefreshView();
        }

        private void RefreshView()
        {
            for (int i = 0; i < _currentScore; i++)
            {
                _stars[i].SetFilled(true);
            }
            for (int i = _currentScore; i < _count; i++)
            {
                _stars[i].SetFilled(false);
            }
        }
    }
}