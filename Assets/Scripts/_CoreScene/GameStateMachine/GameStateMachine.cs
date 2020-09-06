using System;
using System.Collections;
using System.Collections.Generic;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateMachine
    {
        private GameStateController _owner;
        
        private Dictionary<StateTransition, GameState> _transitions;
        
        private GameState _current;

        public GameStateMachine(GameStateController owner, Dictionary<StateTransition, GameState> transitions)
        {
            _owner = owner;
            _transitions = transitions;
        }

        public IEnumerator Initialize(GameState initialState)
        {
            _current = initialState;
            yield return _current.Enter(_owner);
        }
        
        public IEnumerator MoveNext(Command command)
        {
            yield return _current.Exit(_owner);
            _current = GetNext(command);
            yield return _current.Enter(_owner);
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