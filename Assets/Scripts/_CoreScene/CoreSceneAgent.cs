using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    /// <summary>
    /// Add this script to every key scene to be able to start a game from that scene.
    /// This script should be the first executed in any key scene. Add it to Script Execution Order to satisfy that.
    /// </summary>
    [DisallowMultipleComponent]
    public class CoreSceneAgent : MonoBehaviour
    {
        private const string CoreSceneName = "CoreScene";
        private static bool _isCoreSceneInitialized;

        private void Awake()
        {
            if (!_isCoreSceneInitialized)
            {
                SceneManager.LoadScene(CoreSceneName, LoadSceneMode.Additive);
                _isCoreSceneInitialized = true;
            }
        }
    }
}