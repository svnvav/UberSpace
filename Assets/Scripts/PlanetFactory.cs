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
        [SerializeField] private Planet _prefab;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<Planet> _pool;

        [NonSerialized] private Scene _poolScene;
        
        void CreatePools()
        {
            _pool = new List<Planet>();

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
                    _pool.Add(planet);
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
                if (_pool == null)
                {
                    CreatePools();
                }
                
                int lastIndex = _pool.Count - 1;

                if (lastIndex >= 0)
                {
                    instance = _pool[lastIndex];
                    _pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(_prefab);
                    instance.OriginFactory = this;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }

                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_prefab);
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
                if (_pool == null)
                {
                    CreatePools();
                }

                _pool.Add(toRecycle);
                toRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(toRecycle.gameObject);
            }
        }
    }
}