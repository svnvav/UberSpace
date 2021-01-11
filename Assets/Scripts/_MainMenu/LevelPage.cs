using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelPage : MonoBehaviour
    {
        [SerializeField] private Transform _stagesContainer;
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
            
        }
    }
}