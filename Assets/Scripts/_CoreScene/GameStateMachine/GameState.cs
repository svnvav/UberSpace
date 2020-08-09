using System.Collections;

namespace Svnvav.UberSpace.CoreScene
{
    public interface GameState
    {
        IEnumerator Enter(GameStateController controller);
        IEnumerator Exit(GameStateController controller);
    }
}