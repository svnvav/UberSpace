
using System.Collections;
using Catlike.ObjectManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        
        [SerializeField] private CoreDataProvider _coreDataProvider;
        [SerializeField] private GameStateController _gameStateController;

        private void Awake()
        {
            Instance = this;
        }

        public void SaveData()
        {
            _coreDataProvider.UpdateSavedData();
        }

        public void ContinueGame()
        {
            
        }

        public void LoadLevel(int index)
        {
            _gameStateController.GoToLevel(index);
        }

        public void GoToMainMenu()
        {
            _gameStateController.GoToMainMenu();
        }
    }
}