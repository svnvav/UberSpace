
using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private Queue<Order> _queue;

        private Order _current;

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
            //TODO:
        }

        private void TakeOrder()
        {
            _current = _queue.Dequeue();
            
            _current.source.RemoveRace(_current.race);
            
        }

        private struct Order
        {
            public Race race;
            public Planet source, target;
        }
    }
}