using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class RaceFactory : ScriptableObject
    {
        [SerializeField] private Race[] _prefabs;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<Race>[] _pools;

        [NonSerialized] private Scene _poolScene;

         private void CreatePools()
        {
            _pools = new List<Race>[_prefabs.Length];

            for (int i = 0; i < _prefabs.Length; i++)
            {
                _pools[i] = new List<Race>();
            }
            

#if UNITY_EDITOR
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var inactiveRaces = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<Race>());
                foreach (var raceInstance in inactiveRaces)
                {
                    _pools[raceInstance.PrefabId].Add(raceInstance);
                }

                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }

        public Race Get(int prefabId)
        {
            Race instance;

            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }
                var pool = _pools[prefabId];
                int lastIndex = pool.Count - 1;

                if (lastIndex >= 0)
                {
                    instance = pool[lastIndex];
                    pool.RemoveAt(lastIndex);
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

            GameController.Instance.AddRace(instance);
            return instance;
        }

        public void Reclaim(Race toRecycle)
        {
            if (toRecycle.OriginFactory != this)
            {
                Debug.LogError("Tried to reclaim race with wrong factory.");
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