using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    /// <summary>
    /// This script should be the first executed in any key scene.
    /// Add this to Script Execution Order to satisfy that
    /// </summary>
    [DisallowMultipleComponent]
    public class CoreSceneAgent : MonoBehaviour
    {
        private void Awake()
        {
            if (!CoreSceneUtility.IsCoreSceneInitialized)
            {
                CoreSceneUtility.InitializeCoreScene();
            }
        }
    }
}