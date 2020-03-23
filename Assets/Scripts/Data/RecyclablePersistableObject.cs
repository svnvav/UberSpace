using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RecyclablePersistableObject : PersistableObject
    {
        private PrefabFactory _originFactory;
        public PrefabFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                if (_originFactory == null)
                {
                    _originFactory = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }

        private int _prefabId = int.MinValue;
        public int PrefabId
        {
            get => _prefabId;
            set
            {
                if (_prefabId == int.MinValue)
                {
                    _prefabId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }
    }
}