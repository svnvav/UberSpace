using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateController : MonoBehaviour
    {
        private const string MainMenuSceneName = "MainMenu";
        private const string GameSceneName = "Game";
        private const string LevelScenePrefix = "Level";
        
        private GameStateMachine _stateMachine;
        
        private void Start()
        {
            var levelState = new LevelState();
            var menuState = new MenuState();
            var fsmTransitions = new Dictionary<StateTransition, GameState>()
            {
                {new StateTransition(levelState, Command.ToMenu), menuState},
                {new StateTransition(menuState, Command.Play), levelState}
            };
            
            _stateMachine = new GameStateMachine(this, fsmTransitions);
            DefineStartState();
        }

        private void DefineStartState()
        {
            if (IsLevelSceneLoaded())
            {
                
            }
            
        }
        
        private bool IsLevelSceneLoaded()
        {
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith(LevelScenePrefix))
                {
                    return true;
                }
            }

            return false;
        }
    }
}