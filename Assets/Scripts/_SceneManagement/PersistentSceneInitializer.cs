using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace.SceneManagement
{
    public static class PersistentSceneInitializer
    {
        private static string _persistentSceneName = "PersistentScene";
        private static Scene _persistentScene;
        private static bool _isPersistentSceneInitialized;

        public static bool IsPersistentSceneInitialized => _isPersistentSceneInitialized;

        public static void InitializePersistentScene()
        {
            if (_isPersistentSceneInitialized)
            {
                Debug.LogError("Persistent scene is already initialized");
            }

            SceneManager.LoadScene(_persistentSceneName);
            _persistentScene = SceneManager.GetSceneByName(_persistentSceneName);
        }
        
    }
}