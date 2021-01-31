using UnityEngine;

namespace Svnvav.UberSpace
{
    public class LevelPageStarsContainer : MonoBehaviour
    {
        [SerializeField] private UberStar[] _stars;
        public void SetStars(int value)
        {
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetFilled(i < value);
            }
        }
    }
}