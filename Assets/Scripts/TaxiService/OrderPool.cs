using System.Collections.Generic;
using System.Linq;

namespace Svnvav.UberSpace
{
    public class OrderPool
    {
        private List<Order> _pool;

        public OrderPool(int capacity)
        {
            _pool = new List<Order>(capacity);
            
            for (int i = 0; i < capacity; i++)
            {
                _pool.Add(new Order());
            }
        }

        public Order Get(Race race, Planet departure, Planet destination, OrderStatus status = OrderStatus.Queued)
        {
            var order = _pool.FirstOrDefault(o => o.Status == OrderStatus.Completed);

            if (order == null)
            {
                order = new Order();
                _pool.Add(order);
            }
            
            order.Init(race, departure, destination, status);

            return order;
        }
    }
}