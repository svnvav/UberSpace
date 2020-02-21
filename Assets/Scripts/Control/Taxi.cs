using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _takePassengerRadius;
        
        private Queue<Order> _queue;

        private Order _current;

        private void Awake()
        {
            _queue = new Queue<Order>();
        }

        public void AddOrder(Race passenger, Planet from, Planet to)
        {
            _queue.Enqueue(new Order()
            {
                race = passenger,
                source = from,
                target = to
            });
        }

        private void Update()
        {
            if (_current == null && _queue.Count > 0)
            {
                TakeOrder();
            }
            
        }

        private void TakeOrder()
        {
            _current = _queue.Dequeue();
            _current.source.RemoveRace(_current.race);
        }

        private class Order
        {
            public Race race;
            public Planet source, target;
        }
    }
}