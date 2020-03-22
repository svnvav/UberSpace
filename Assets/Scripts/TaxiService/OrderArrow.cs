using UnityEngine;

namespace Svnvav.UberSpace
{
    public class OrderArrow : MonoBehaviour, IRecyclable
    {
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
        
        private IReclaimer _originFactory;
        public IReclaimer OriginFactory
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
        public GameObject RecyclableGameObject => gameObject;
    }
}