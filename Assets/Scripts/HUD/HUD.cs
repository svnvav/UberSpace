using UnityEngine;

namespace Svnvav.UberSpace
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private RacesInfoCanvas _racesInfoCanvas;

        public void AddRaceInfo(Race race)
        {
            _racesInfoCanvas.AddRaceInfo(race);
        }
    }
}