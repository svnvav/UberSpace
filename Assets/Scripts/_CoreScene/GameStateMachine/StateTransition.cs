namespace Svnvav.UberSpace.CoreScene
{
    public class StateTransition
    {
        readonly GameState CurrentState;
        readonly Command Command;

        public StateTransition(GameState currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }
}