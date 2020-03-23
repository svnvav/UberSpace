using UnityEngine;

namespace Svnvav.UberSpace
{
    public interface IReclaimer<T> where T : Object, IRecyclable
    {
        void Reclaim(T recyclable);
    }
}