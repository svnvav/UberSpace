using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class ScenarioItem : MonoBehaviour
    {
        public abstract bool Progress(float deltaTime);
    }
}