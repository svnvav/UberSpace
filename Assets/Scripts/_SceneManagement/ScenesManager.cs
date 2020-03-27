using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    public class ScenesManager : MonoBehaviour
    {
         /// <summary>
        /// The scenes manager instance.
        /// </summary>
        public static ScenesManager Instance;
        /// <summary>
        /// Current scene name.
        /// </summary>
        private string _currentSceneName;
        /// <summary>
        /// Next scene name.
        /// </summary>
        private string _nextSceneName;
        /// <summary>
        /// Unload resource task.
        /// </summary>
        private AsyncOperation _resourceUnloadTask;
        /// <summary>
        /// Load scene task.
        /// </summary>
        private AsyncOperation _sceneLoadTask;
        /// <summary>
        /// Current scene state.
        /// </summary>
        private SceneState _sceneState;
        /// <summary>
        /// Update delegates array.
        /// </summary>
        private UpdateDelegate[] _updateDelegates;
        private void Awake()
        {
            // Keep alive between scene changes
            DontDestroyOnLoad(gameObject);
            // Setup the array of updateDelegates
            _updateDelegates = new UpdateDelegate[(int) SceneState.Count];
            _updateDelegates[(int) SceneState.Reset] = UpdateSceneReset;
            _updateDelegates[(int) SceneState.Preload] = UpdateScenePreload;
            _updateDelegates[(int) SceneState.Load] = UpdateSceneLoad;
            _updateDelegates[(int) SceneState.Unload] = UpdateSceneUnload;
            _updateDelegates[(int) SceneState.PostLoad] = UpdateScenePostLoad;
            _updateDelegates[(int) SceneState.Ready] = UpdateSceneReady;
            _updateDelegates[(int) SceneState.Run] = UpdateSceneRun;
            _nextSceneName = SceneManager.GetActiveScene().name;
            _sceneState = SceneState.Reset;
        }
        private void OnDestroy()
        {
            if (_updateDelegates == null)
                return;
            for (var i = 0; i < (int) SceneState.Count; i++)
                _updateDelegates[i] = null;
        }
        private void Start()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
        /// <summary>
        /// Scene switch.
        /// </summary>
        /// <param name="nextSceneName"> Next scene name. </param>
        public static void SwitchScene(string nextSceneName)
        {
            if (Instance == null)
                return;
            if (Instance._currentSceneName != nextSceneName)
                Instance._nextSceneName = nextSceneName;
        }
        private void Update()
        {
            _updateDelegates[(int) _sceneState].Invoke();
        }
        /// <summary>
        /// Scene loading
        /// </summary>
        private void UpdateSceneLoad()
        {
            if (_sceneLoadTask.isDone)
                _sceneState = SceneState.Unload;
            {
                //update scene loading progress
            }
        }
        /// <summary>
        /// Actions immediately after loading
        /// </summary>
        private void UpdateScenePostLoad()
        {
            _currentSceneName = _nextSceneName;
            _sceneState = SceneState.Ready;
        }
        /// <summary>
        /// Start to load scene asynchronously
        /// </summary>
        private void UpdateScenePreload()
        {
            _sceneLoadTask = SceneManager.LoadSceneAsync(_nextSceneName);
            _sceneState = SceneState.Load;
        }
        /// <summary>
        /// Actions immediately before running
        /// </summary>
        private void UpdateSceneReady()
        {
            // if you don't use assets in current state
            // you can use that
            // GC.Collect();
            _sceneState = SceneState.Run;
        }
        /// <summary>
        /// Rest scene.
        /// </summary>
        private void UpdateSceneReset()
        {
            GC.Collect();
            _sceneState = SceneState.Preload;
        }
        /// <summary>
        /// Wait for scene change
        /// </summary>
        private void UpdateSceneRun()
        {
            if (_currentSceneName != _nextSceneName)
                _sceneState = SceneState.Reset;
        }
        /// <summary>
        /// Cleanup unused resources.
        /// </summary>
        private void UpdateSceneUnload()
        {
            if (_resourceUnloadTask == null)
            {
                _resourceUnloadTask = Resources.UnloadUnusedAssets();
            }
            else
            {
                if (!_resourceUnloadTask.isDone)
                    return;
                _resourceUnloadTask = null;
                _sceneState = SceneState.PostLoad;
            }
        }
        /// <summary>
        /// Scene state.
        /// </summary>
        private enum SceneState
        {
            Reset, //GC.Collect.
            Preload, // Start to loading level asynchronously.
            Load, // Upload until loading.
            Unload, // Resources.UnloadUnusedAssets.
            PostLoad, // Update currentSceneName.
            Ready, // GC.Collect (safely), Things before beginning to play (SplashScreen).
            Run, // Stay here until CurrentLevelName != nextLevelName (Changing by public method).
            Count // Count of scene states.
        }
        /// <summary>
        /// Update delegate.
        /// </summary>
        private delegate void UpdateDelegate();
    }
}