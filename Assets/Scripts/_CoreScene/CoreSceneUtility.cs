using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public static class CoreSceneUtility
    {
        private static string _coreSceneName = "CoreScene";
        private static Scene _coreScene;
        private static bool _isCoreSceneInitialized;

        public static bool IsCoreSceneInitialized => _isCoreSceneInitialized;

        public static void InitializeCoreScene()
        {
            if (_isCoreSceneInitialized)
            {
                Debug.LogError("Core scene is already initialized");
            }

            SceneManager.LoadScene(_coreSceneName, LoadSceneMode.Additive);
            _coreScene = SceneManager.GetSceneByName(_coreSceneName);
        }
        
    }
}