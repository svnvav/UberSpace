namespace Svnvav.UberSpace.CoreScene
{
    public class StateTransition
    {
        private readonly IGameState _currentState;
        private readonly Command _command;

        public StateTransition(IGameState currentState, Command command)
        {
            _currentState = currentState;
            _command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * _currentState.GetHashCode() + 31 * _command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this._currentState == other._currentState && this._command == other._command;
        }
    }
}