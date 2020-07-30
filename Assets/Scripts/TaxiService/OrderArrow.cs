using UnityEngine;

namespace Svnvav.UberSpace
{
    public class OrderArrow : RecyclableMonoBehaviour
    {
        [SerializeField] private Arrow _arrow;

        public Arrow Arrow => _arrow;

        public void Reclaim()
        {
            OriginFactory.Reclaim(this);
        }
    }
}