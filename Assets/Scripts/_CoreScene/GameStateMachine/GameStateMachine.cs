using System;
using System.Collections.Generic;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateMachine
    {
        private GameStateController _owner;
        
        private Dictionary<StateTransition, GameState> _transitions;
        
        private GameState _current;

        public GameStateMachine(GameStateController owner, GameState initialState)
        {
            _owner = owner;
            
            _current = initialState;
            _current.Enter(_owner);
        }
        
        public GameState MoveNext(Command command)
        {
            _current.Exit(_owner);
            _current = GetNext(command);
            _current.Enter(_owner);
            return _current;
        }
        
        private GameState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(_current, command);
            GameState nextState;
            if (!_transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + _current + " -> " + command);
            return nextState;
        }

       
    }
}