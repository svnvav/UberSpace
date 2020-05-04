using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;

        //[SerializeField] private SceneSwitcher _scenesSwitcher;

        private void Awake()
        {
            Instance = this;
        }

        public void ContinueGame()
        {
            
        }

        public void StartLevel(int index)
        {
            //_scenesSwitcher.SwitchScene($"Game");
        }
        
        
    }
}