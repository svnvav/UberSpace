using System;
using System.Collections;
using System.Collections.Generic;

namespace Svnvav.UberSpace.CoreScene
{
    public class GameStateMachine
    {
        private GameStateController _owner;
        
        private Dictionary<StateTransition, IGameState> _transitions;
        
        private IGameState _current;

        public GameStateMachine(GameStateController owner, Dictionary<StateTransition, IGameState> transitions)
        {
            _owner = owner;
            _transitions = transitions;
        }

        public IEnumerator Initialize(IGameState initialState)
        {
            _current = initialState;
            yield return _current.Enter(_owner);
        }
        
        public IEnumerator MoveNext(Command command)
        {
            //TODO: mb move loading progress processing from states to here?
            yield return _current.Exit(_owner);
            GC.Collect();
            _current = GetNext(command);
            yield return _current.Enter(_owner);
        }
        
        private IGameState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(_current, command);
            IGameState nextState;
            if (!_transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + _current + " -> " + command);
            return nextState;
        }

       
    }
}