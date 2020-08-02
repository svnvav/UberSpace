namespace Svnvav.UberSpace.CoreScene
{
    public interface GameState
    {
        void Enter(GameStateController controller);
        void Exit(GameStateController controller);
    }
}