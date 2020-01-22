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
        [SerializeField] private RaceInstance _prefab;
        [SerializeField] private RaceInfo[] _infos;

        public RaceInfo[] Infos => _infos;

        [SerializeField] private bool _recycle = false;

        [NonSerialized] private List<RaceInstance> _pool;

        [NonSerialized] private Scene _poolScene;

        void CreatePools()
        {
            _pool = new List<RaceInstance>();

#if UNITY_EDITOR
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                var inactiveRaces = _poolScene
                    .GetRootGameObjects()
                    .Where(go => !go.activeSelf)
                    .Select(go => go.GetComponent<RaceInstance>());
                foreach (var raceInstance in inactiveRaces)
                {
                    _pool.Add(raceInstance);
                }

                return;
            }
#endif

            _poolScene = SceneManager.CreateScene(name);
        }

        public RaceInstance Get(int infoId)
        {
            RaceInstance instance;

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
                    instance.InfoId = infoId;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }

                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(_prefab);
                instance.OriginFactory = this;
                instance.InfoId = infoId;
            }

            GameController.Instance.AddRace(instance);
            return instance;
        }

        public void Reclaim(RaceInstance toRecycle)
        {
            if (toRecycle.OriginFactory != this)
            {
                Debug.LogError("Tried to reclaim race with wrong factory.");
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