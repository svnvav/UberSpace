using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _takePassengerRadius;
        
        private Queue<Order> _ordersQueue; //TODO: delete dead races,
        public Order[] Orders => _ordersQueue.ToArray();

        private Order _current;
        private Race _passenger;

        private void Awake()
        {
            _ordersQueue = new Queue<Order>();
        }

        public void AddOrder(Race passenger, Planet departure, Planet destination)
        {
            _ordersQueue.Enqueue(new Order(passenger, departure, destination));
        }

        private void Update()
        {
            if (_current == null && _ordersQueue.Count == 0)
            {
                Idle();
                return;
            }
            if (_current == null && _ordersQueue.Count > 0)
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
            _current = _ordersQueue.Dequeue();
        }

        private void GoToDeparture()
        {
            var departurePoint = _current.Departure.transform.position;
            MoveTo(departurePoint);
            
            if (Vector3.SqrMagnitude(transform.position - departurePoint) <
                _takePassengerRadius * _takePassengerRadius)
            {
                _passenger = _current.Race;
                _current.Departure.RemoveRace(_passenger);
            }
        }
        
        private void GoToDestination()
        {
            var destinationPoint = _current.Destination.transform.position;
            MoveTo(destinationPoint);
            
            if (Vector3.SqrMagnitude(transform.position - destinationPoint) <
                _takePassengerRadius * _takePassengerRadius)
            {
                _current.Destination.AddRace(_passenger);
                _passenger = null;
                _current = null;
            }
        }

        private void MoveTo(Vector3 destination)
        {
            transform.LookAt(destination, Vector3.forward);
            transform.Translate(_speed * Time.deltaTime * transform.forward, Space.World);
        }
    }
}