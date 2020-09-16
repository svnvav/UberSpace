using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class ScenarioStageBase : MonoBehaviour
    {
        public abstract float Duration { get; }

        public abstract void Progress(float deltaTime);
        
        public abstract void SetTime(float progress);
    }
}