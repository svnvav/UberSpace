using System.Collections.Generic;

namespace Svnvav.UberSpace
{
    public class OrderArrowsChain
    {
        
        public OrderArrowsChain(OrderLinkedQueue orders)
        {
            orders.OnEnqueue += OnEnqueue;
            orders.OnDequeue += OnDequeue;

        }

        private void OnEnqueue()
        {
            //TODO:
        }

        private void OnDequeue()
        {
            //TODO:
        }
    }
}