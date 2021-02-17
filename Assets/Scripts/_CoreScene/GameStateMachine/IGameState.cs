using System.Collections;

namespace Svnvav.UberSpace.CoreScene
{
    public interface IGameState
    {
        IEnumerator Enter(GameStateController controller);
        IEnumerator Exit(GameStateController controller);
    }
}