using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Svnvav.UberSpace
{
    public class PrefabPool
    {
        private Type _type;

        private List<Object>[] _pools;

        public PrefabPool(Type type, int prefabsCount)
        {
            _type = type;
        }

        public void Push(Object obj)
        {
            
        }
    }
}