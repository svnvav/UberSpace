using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class PlanetFactory : ScriptableObject
    {
        [SerializeField] private Planet[] _prefabs;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<Planet>[] _pools;

        [NonSerialized] private Scene _poolScene;
        
        void CreatePools()
        {
            _pools = new List<Planet>[_prefabs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<Planet>();
            }

#if UNITY_EDITOR
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var inactiveObjects = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<Planet>());
                foreach (var planet in inactiveObjects)
                {
                    _pools[planet.PrefabId].Add(planet);
                }
                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }
        
        public Planet Get(int prefabId)
        {
            Planet instance;

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
                    instance.OriginFactory = this;
                    instance.PrefabId = prefabId;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }

                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_prefabs[prefabId]);
                instance.OriginFactory = this;
                instance.PrefabId = prefabId;
            }

            GameController.Instance.AddPlanet(instance);
            return instance;
        }
        
        public void Reclaim(Planet toRecycle)
        {
            if (toRecycle.OriginFactory != this) {
                Debug.LogError("Tried to reclaim planet with wrong factory.");
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