using UnityEngine;

namespace Svnvav.UberSpace
{
    public class UberStars : MonoBehaviour
    {
        [SerializeField] private UberStar _starPrefab;
        [SerializeField] private readonly int _count;
        [SerializeField] private int _currentScore;

        private UberStar[] _stars;

        private void Awake()
        {
            _stars = new UberStar[_count];
            for (int i = 0; i < _count; i++)
            {
                _stars[i] = Instantiate(_starPrefab, transform);
            }
            RefreshView();
        }

        public void Decrease()
        {
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