using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class PrefabFactory : ScriptableObject
    {
        [SerializeField] private RecyclableMonoBehaviour[] _prefabs;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<RecyclableMonoBehaviour>[] _pools;

        [NonSerialized] private Scene _poolScene;
        
        [NonSerialized] private Type _targetType;

        public Type TargetType => _targetType;

        public void Initialize(Type type)
        {
            _targetType = type;
        }
        
        private void CreatePools()
        {
            _pools = new List<RecyclableMonoBehaviour>[_prefabs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<RecyclableMonoBehaviour>();
            }

#if UNITY_EDITOR
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var inactiveObjects = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<RecyclableMonoBehaviour>());
                foreach (var instances in inactiveObjects)
                {
                    _pools[instances.PrefabId].Add(instances);
                }
                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }
        
        public T Get<T>(int prefabId) where T: RecyclableMonoBehaviour
        {
            if (typeof(T) != _targetType)
            {
                Debug.LogError($"Trying to get object of type {typeof(T).Name} from pool of type {_targetType.Name}");
            }
            
            T instance;

            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }
                
                int lastIndex = _pools[prefabId].Count - 1;

                if (lastIndex >= 0)
                {
                    instance = (T)_pools[prefabId][lastIndex];
                    _pools[prefabId].RemoveAt(lastIndex);
                }
                else
                {
                    instance = (T)Instantiate(_prefabs[prefabId]);
                    instance.OriginFactory = this;
                    instance.PrefabId = prefabId;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }

                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = (T)Instantiate(_prefabs[prefabId]);
                instance.OriginFactory = this;
                instance.PrefabId = prefabId;
            }
            
            return instance;
        }
        
        public void Reclaim(RecyclableMonoBehaviour toRecycle)
        {
            if (toRecycle.OriginFactory != this)
            {
                Debug.LogError("Tried to reclaim object with wrong factory.");
                return;
            }
            
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                _pools[toRecycle.PrefabId].Add(toRecycle);
                toRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(toRecycle.gameObject);
            }
        }
    }
}