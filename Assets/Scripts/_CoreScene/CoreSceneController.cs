using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        private static string _mainMenuSceneName = "MainMenu";
        private static string _gameSceneName = "Game";
        private static string _levelScenePrefix = "Level";

        //[SerializeField] private SceneSwitcher _scenesSwitcher;

        private GameState _currentGameState = GameState.Loading;
        private GameState _targetGameState;
        private Scene _stateScene;
        private string _levelPostfix;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_currentGameState != _targetGameState)
            {
                
            }
        }

        public void ContinueGame()
        {
            
        }

        public void StartLevel(string postfix)
        {
            _targetGameState = GameState.Game;
        }

        public void GoToMainMenu()
        {
            _targetGameState = GameState.Menu;
        }
        
        
        private enum GameState
        {
            Menu,
            Loading,
            Game
        }
    }
}