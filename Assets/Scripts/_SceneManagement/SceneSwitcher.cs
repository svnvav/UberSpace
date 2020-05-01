using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Svnvav.UberSpace.SceneManagement
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Text _progress;
        
        private string _currentSceneName;

        private string _nextSceneName;

        private AsyncOperation _resourceUnloadTask;

        private AsyncOperation _sceneLoadTask;
        /// <summary>
        /// Current scene state.
        /// </summary>
        private SceneState _sceneState;

        private UpdateDelegate[] _updateDelegates;
        public void Initialize(string initSceneName)
        {
            _updateDelegates = new UpdateDelegate[(int) SceneState.Count];
            _updateDelegates[(int) SceneState.Reset] = UpdateSceneReset;
            _updateDelegates[(int) SceneState.Preload] = UpdateScenePreload;
            _updateDelegates[(int) SceneState.Load] = UpdateSceneLoad;
            _updateDelegates[(int) SceneState.Unload] = UpdateSceneUnload;
            _updateDelegates[(int) SceneState.PostLoad] = UpdateScenePostLoad;
            _updateDelegates[(int) SceneState.Ready] = UpdateSceneReady;
            _updateDelegates[(int) SceneState.Run] = UpdateSceneRun;
            _nextSceneName = initSceneName;
            _sceneState = SceneState.Reset;
        }
        private void OnDestroy()
        {
            if (_updateDelegates == null)
                return;
            for (var i = 0; i < (int) SceneState.Count; i++)
                _updateDelegates[i] = null;
        }
        
        public void SwitchScene(string nextSceneName)
        {
            if (_currentSceneName != nextSceneName)
            {
                _nextSceneName = nextSceneName;
            }
        }
        private void Update()
        {
            _updateDelegates[(int) _sceneState].Invoke();
        }
        
        /// <summary>
        /// Rest scene.
        /// </summary>
        private void UpdateSceneReset()
        {
            _loadingScreen.SetActive(true);
            GC.Collect();
            _sceneState = SceneState.Preload;
        }
        /// <summary>
        /// Start to load scene asynchronously
        /// </summary>
        private void UpdateScenePreload()
        {
            _sceneLoadTask = SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
            _sceneState = SceneState.Load;
        }
        /// <summary>
        /// Scene loading
        /// </summary>
        private void UpdateSceneLoad()
        {
            if (_sceneLoadTask.isDone)
                _sceneState = SceneState.Unload;
            {
                _progress.text = $"{(int)Mathf.Floor(_sceneLoadTask.progress * 100)} %";
            }
        }
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
        /// Actions immediately after loading
        /// </summary>
        private void UpdateScenePostLoad()
        {
            _loadingScreen.SetActive(false);
            _currentSceneName = _nextSceneName;
            _sceneState = SceneState.Ready;
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