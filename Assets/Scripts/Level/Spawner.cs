using Catlike.ObjectManagement;

namespace Svnvav.UberSpace
{
    public abstract class Spawner : PersistableObject
    {
        public abstract void Progress(float deltaTime);
    }
}