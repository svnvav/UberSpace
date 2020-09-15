using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        
        [SerializeField] private CoreDataProvider _coreDataProvider;
        [SerializeField] private GameStateController _gameStateController;

        public CoreDataProvider CoreDataProvider => _coreDataProvider;
        public GameStateController GameStateController => _gameStateController;

        private void Awake()
        {
            Instance = this;
        }

        public void ContinueGame()
        {
            _gameStateController.GoToLevel(CoreDataProvider.LastLoadedLevelPostfix, true);
            //TODO: fix planets veil bug after loading
        }

        public void LoadLevel(int index)
        {
            _gameStateController.GoToLevel(index, false);
        }

        public void GoToMainMenu()
        {
            _gameStateController.GoToMainMenu();
        }
    }
}