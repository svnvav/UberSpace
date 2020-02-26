using System.Collections.Generic;
using System.Linq;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : PersistableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _takePassengerRadius;

        private Queue<Order> _ordersQueue; //TODO: delete dead races,
        private OrderPool _pool;

        private void Awake()
        {
            _ordersQueue = new Queue<Order>();
            _pool = new OrderPool(1);
        }

        private void Start()
        {
            GameController.Instance.RegisterOnRemovePlanet(RemoveOrdersWithPlanet);
        }

        public void AddOrder(Race passenger, Planet departure, Planet destination)
        {
            _ordersQueue.Enqueue(_pool.Get(passenger, departure, destination));
        }

        public void RemoveOrdersWithPlanet(Planet planet)
        {
            //TODO: consider dragndrop from taxiship
            if (_ordersQueue.Count == 0) return;

            Order canceled = null;

            var current = _ordersQueue.Peek();
            if (current.Status == OrderStatus.Executing)
            {
                if (current.Destination == planet)
                {
                    canceled = _ordersQueue.LastOrDefault(o => o.Destination == current.Departure);
                    canceled?.Cancel();
                    //return back
                    current.Cancel();
                }
            }

            foreach (var order in _ordersQueue)
            {
                if (order.Status != OrderStatus.Executing && 
                    (order.Departure == planet || order.Destination == planet)
                    )
                {
                    order.Cancel();
                }
            }
            
            var newOrders = _ordersQueue
                .Where(order => order.Status != OrderStatus.Completed); //TODO: mb implement with linked list?
            _ordersQueue = new Queue<Order>(newOrders);
        }

        public void GameUpdate(float deltaTime)
        {
            if (_ordersQueue.Count == 0)
            {
                Idle();
                return;
            }

            var order = _ordersQueue.Peek();
            switch (order.Status)
            {
                case OrderStatus.Queued:
                    order.Take();
                    break;
                case OrderStatus.Taken:
                    MoveTo(order.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - order.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        order.StartExecuting();
                    }

                    break;
                case OrderStatus.Executing:
                    MoveTo(order.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - order.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        order.Complete();
                        _ordersQueue.Dequeue();
                    }

                    break;
            }
        }

        private void Idle()
        {
        }


        private void MoveTo(Vector3 destination, float deltaTime)
        {
            transform.LookAt(destination, Vector3.forward);
            transform.Translate(_speed * deltaTime * transform.forward, Space.World);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(transform.localRotation);

            var ordersToSave = new List<Order>(_ordersQueue);

            writer.Write(ordersToSave.Count);

            foreach (var order in ordersToSave)
            {
                writer.Write(order.Race.SaveIndex);
                writer.Write(order.Departure.SaveIndex);
                writer.Write(order.Destination.SaveIndex);
                writer.Write((int) order.Status);
            }
        }

        public override void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            transform.localRotation = reader.ReadQuaternion();

            _ordersQueue.Clear();
            var count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                var race = GameController.Instance.Races[reader.ReadInt()];
                var departure = GameController.Instance.Planets[reader.ReadInt()];
                var destination = GameController.Instance.Planets[reader.ReadInt()];
                var status = (OrderStatus) reader.ReadInt();

                _ordersQueue.Enqueue(_pool.Get(race, departure, destination, status));
            }
        }
    }
}