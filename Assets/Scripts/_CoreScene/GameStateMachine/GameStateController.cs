using System;
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
            DefineStartState();
        }

        private void DefineStartState()
        {
            if (IsLevelSceneLoaded())
            {
                _stateMachine = new GameStateMachine(this);
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