using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class Spawner : MonoBehaviour
    {
        public abstract void Progress(float deltaTime);
    }
}