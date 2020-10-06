using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private RacesInfoCanvas _racesInfoCanvas;
        [SerializeField] private UberStarsCanvas _uberStarsCanvas;

        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _veil;

        public void AddRaceInfo(Race race)
        {
            _racesInfoCanvas.AddRaceInfo(race);
        }
        
        public void RemoveRaceInfo(Race race)
        {
            _racesInfoCanvas.RemoveRaceInfo(race);
        }

        public void DecreaseStar()
        {
            _uberStarsCanvas.Decrease();
        }

        public void OnPause()
        {
            _veil.SetActive(true);
            _pausePanel.SetActive(true);
        }
        
        public void OnUnpause()
        {
            _pausePanel.SetActive(false);
            _veil.SetActive(false);
        }
        
        public void Unpause()
        {
            GameController.Instance.Unpause();
        }

        public void GoToMainMenu()
        {
            CoreSceneController.Instance.GoToMainMenu();
        }
    }
}