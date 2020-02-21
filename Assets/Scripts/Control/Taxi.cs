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
        private Race _passenger;

        private void Awake()
        {
            _queue = new Queue<Order>();
        }

        public void AddOrder(Race passenger, Planet departure, Planet destination)
        {
            _queue.Enqueue(new Order()
            {
                race = passenger,
                departure = departure,
                destination = destination
            });
        }

        private void Update()
        {
            if (_current == null && _queue.Count == 0)
            {
                Idle();
                return;
            }
            if (_current == null && _queue.Count > 0)
            {
                TakeOrder();
            }

            if (_passenger == null)
            {
                GoToDeparture();
            }
            else
            {
                GoToDestination();
            }
        }

        private void Idle()
        {
            
        }
        
        private void TakeOrder()
        {
            _current = _queue.Dequeue();
        }

        private void GoToDeparture()
        {
            var departurePoint = _current.departure.transform.position;
            MoveTo(departurePoint);
            
            if (Vector3.SqrMagnitude(transform.position - departurePoint) <
                _takePassengerRadius * _takePassengerRadius)
            {
                _passenger = _current.race;
                _current.departure.RemoveRace(_passenger);
            }
        }
        
        private void GoToDestination()
        {
            var destinationPoint = _current.destination.transform.position;
            MoveTo(destinationPoint);
            
            if (Vector3.SqrMagnitude(transform.position - destinationPoint) <
                _takePassengerRadius * _takePassengerRadius)
            {
                _current.destination.AddRace(_passenger);
                _passenger = null;
                _current = null;
            }
        }

        private void MoveTo(Vector3 destination)
        {
            transform.LookAt(destination, Vector3.forward);
            transform.Translate(_speed * Time.deltaTime * transform.forward, Space.World);
        }

        private class Order
        {
            public Race race;
            public Planet departure, destination;
        }
    }
}