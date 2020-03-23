using UnityEngine;

namespace Svnvav.UberSpace
{
    public interface IRecyclable
    {
        void SetOriginFactory<T>(PrefabGenericFactory<T> originGenericFactory) where T : Object, IRecyclable;

        int PrefabId { get; set; }
        
        GameObject RecyclableGameObject { get; }
    }
}