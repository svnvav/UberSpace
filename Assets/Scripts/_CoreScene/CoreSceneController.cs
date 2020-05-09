using System;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        private static string _mainMenuSceneName = "MainMenu";
        private static string _gameSceneName = "Game";
        private static string _levelScenePrefix = "Level";

        //[SerializeField] private SceneSwitcher _scenesSwitcher;

        private GlobalGameState _currentState;

        public void SetState(GlobalGameState state)
        {
            _currentState = state;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            throw new NotImplementedException();
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