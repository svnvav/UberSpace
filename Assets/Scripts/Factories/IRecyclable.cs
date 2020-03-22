using UnityEngine;

namespace Svnvav.UberSpace
{
    public interface IRecyclable
    {
        IReclaimer OriginFactory { get; set; }
        int PrefabId { get; set; }
        
        GameObject RecyclableGameObject { get; }
    }
}