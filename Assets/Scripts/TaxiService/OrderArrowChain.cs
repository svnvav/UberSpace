using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class OrderArrowChain
    {
        private List<OrderArrow> _betweenOrdersArrows, _orderArrows;

        private LinkedQueue<Order> _orders;

        private Transform _taxiTransform;

        public OrderArrowChain(LinkedQueue<Order> orders, Transform taxiTransform)
        {
            _orders = orders;
            _taxiTransform = taxiTransform;
            
            _betweenOrdersArrows = new List<OrderArrow>();
            _orderArrows = new List<OrderArrow>();
        }

        public void GameUpdate()
        {
            if(_orders.Count == 0)
                return;

            var lastPosition = _taxiTransform.position;
            var betweenIndex = 0;
            var index = 0;
            
            foreach (var order in _orders)
            {
                if (order.Status != OrderStatus.Executing)
                {
                    var betweenArrow = GetOrderArrow(betweenIndex++, 1, _betweenOrdersArrows);
                    betweenArrow.Arrow.SetPosition(0, lastPosition);
                    lastPosition = order.Departure.transform.position;
                    betweenArrow.Arrow.SetPosition(1, lastPosition);
                }

                var orderArrow = GetOrderArrow(index++, 0, _orderArrows);
                orderArrow.Arrow.SetPosition(0, lastPosition);
                lastPosition = order.Destination.transform.position;
                orderArrow.Arrow.SetPosition(1, lastPosition);
            }
            
            FlushArrows(betweenIndex, _betweenOrdersArrows);
            FlushArrows(index, _orderArrows);
        }

        private OrderArrow GetOrderArrow(int i, int prefabId, List<OrderArrow> list)
        {
            while (i >= list.Count)
            {
                list.Add(FactoriesManager.Instance.Get<OrderArrow>(prefabId));
            }

            return list[i];
        }
        
        
        private void FlushArrows(int lastIndex, List<OrderArrow> list)
        {
            if (lastIndex == list.Count)
            {
                return;
            }

            for (int i = lastIndex; i < list.Count; i++)
            {
                list[i].Reclaim();
            }
            
            list.RemoveRange(lastIndex, list.Count - lastIndex);
        }
    }
}