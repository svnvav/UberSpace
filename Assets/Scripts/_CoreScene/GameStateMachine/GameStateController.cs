using System;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateController : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        
        private void Start()
        {
            DefineStartState();
        }

        private void DefineStartState()
        {
            _stateMachine = new GameStateMachine(this, );
        }
    }
}