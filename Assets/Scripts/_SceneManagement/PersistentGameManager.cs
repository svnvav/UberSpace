using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    public class PersistentGameManager : MonoBehaviour
    {
        public static PersistentGameManager Instance;

        [SerializeField] private SceneSwitcher _scenesSwitcher;

        private void Awake()
        {
            Instance = this;
            _scenesSwitcher.Initialize("MainMenu");
        }

        public void ContinueGame()
        {
            
        }

        public void StartLevel(int index)
        {
            _scenesSwitcher.SwitchScene($"Level{index}");
        }
        
        
    }
}