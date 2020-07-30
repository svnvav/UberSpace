
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class FactoriesManager : MonoBehaviour
    {
        public static FactoriesManager Instance;

        [SerializeField] private PrefabFactory _arrowFactory;

        private Dictionary<Type, PrefabFactory> _factories;

        private void Awake()
        {
            Instance = this;
            _factories = new Dictionary<Type, PrefabFactory>();
            
            _arrowFactory.Initialize(typeof(OrderArrow));
            _factories.Add(_arrowFactory.TargetType, _arrowFactory);
        }

        public T Get<T>(int prefabId) where T : RecyclableMonoBehaviour
        {
            PrefabFactory factory;
            if (!_factories.TryGetValue(typeof(T), out factory))
            {
                Debug.LogError($"There is no initialized factory for type: {typeof(T).Name}");
            }

            return factory.Get<T>(prefabId);
        }
    }
}