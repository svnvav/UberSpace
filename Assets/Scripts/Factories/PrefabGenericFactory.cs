using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Svnvav.UberSpace
{
    public abstract class PrefabGenericFactory<T> : ScriptableObject, IReclaimer<T> where T : Object, IRecyclable
    {
        [SerializeField] private T[] _prefabs;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<T>[] _pools;

        [NonSerialized] private Scene _poolScene;

        private void CreatePools()
        {
            _pools = new List<T>[_prefabs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<T>();
            }

#if UNITY_EDITOR
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var inactiveObjects = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<T>());
                foreach (var planet in inactiveObjects)
                {
                    _pools[planet.PrefabId].Add(planet);
                }
                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }
        
        public T Get(int prefabId)
        {
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
                    instance = _pools[prefabId][lastIndex];
                    _pools[prefabId].RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(_prefabs[prefabId]);
                    instance.SetOriginFactory(this);
                    instance.PrefabId = prefabId;
                    SceneManager.MoveGameObjectToScene(instance.RecyclableGameObject, _poolScene);
                }

                instance.RecyclableGameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_prefabs[prefabId]);
                instance.SetOriginFactory(this);
                instance.PrefabId = prefabId;
            }
            
            return instance;
        }
        
        public void Reclaim(T toRecycle)
        {
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                _pools[toRecycle.PrefabId].Add(toRecycle);
                toRecycle.RecyclableGameObject.SetActive(false);
            }
            else
            {
                Destroy(toRecycle.RecyclableGameObject);
            }
        }
    }
}