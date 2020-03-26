using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class RaceInfo : MonoBehaviour
    {
        [SerializeField] private Image _raceIcon;

        private Race _race;

        public void Initialize(Race race)
        {
            _race = race;
            _raceIcon.sprite = _race.HudInfoSprite;
        }
    }
}