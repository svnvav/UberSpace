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
                var inactiveShapes = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<Planet>());
                foreach (var planet in inactiveShapes)
                {
                    _pools[planet.IdInFactory].Add(planet);
                }
                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }
        
        public Planet Get(int idInFactory = 0)
        {
            Planet instance;

            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                List<Planet> pool = _pools[idInFactory];
                int lastIndex = pool.Count - 1;

                if (lastIndex >= 0)
                {
                    instance = pool[lastIndex];
                    pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(_prefabs[idInFactory]);
                    instance.OriginFactory = this;
                    instance.IdInFactory = idInFactory;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }

                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_prefabs[idInFactory]);
                instance.IdInFactory = idInFactory;
            }

            GameController.Instance.AddPlanet(instance);
            return instance;
        }
        
        public void Reclaim(Planet toRecycle)
        {
            if (toRecycle.OriginFactory != this) {
                Debug.LogError("Tried to reclaim shape with wrong factory.");
                return;
            }
            
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                _pools[toRecycle.IdInFactory].Add(toRecycle);
                toRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(toRecycle.gameObject);
            }
        }
    }
}