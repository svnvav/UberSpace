using System;
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
        //public Order[] Orders => _ordersQueue.ToArray();

        //private Order _current;
        //private Race _passenger;
        private OrderPool _pool;

        private void Awake()
        {
            _ordersQueue = new Queue<Order>();
            _pool = new OrderPool(5);
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
            /*Order canceled = null;
            if (_current != null)
            {
                if (_passenger != null)
                {
                    if (_current.Destination == planet)
                    {
                        //return back
                        _current = new Order(_passenger, _current.Departure, _current.Departure); 

                        canceled = _ordersQueue.LastOrDefault(o => o.Destination == _current.Destination);
                        if (canceled != null)
                        {
                            canceled.Destination.RemoveRaceToArrive(canceled.Client);
                            canceled.Departure.RemoveRaceToDeparture(canceled.Client);
                        }
                        
                        _current.Destination.AddRaceToArrive(_passenger);
                        //TODO: consider dragndrop from taxiship
                    }
                }
                else
                {
                    if (_current.Departure == planet || _current.Destination == planet)
                    {
                        _current = null;
                    }
                }
            }

            var newOrders = _ordersQueue
                .Where(order => order.Departure != planet && order.Destination != planet && order != canceled); //TODO: mb implement with linked list?
            _ordersQueue = new Queue<Order>(newOrders);*/
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
                    }
                    break;
            }
        }

        private void Idle()
        {
        }

        private void GoToDestination(float deltaTime)
        {
            var destinationPoint = _current.Destination.transform.position;
            MoveTo(destinationPoint, deltaTime);

            if (Vector3.SqrMagnitude(transform.position - destinationPoint) <
                _takePassengerRadius * _takePassengerRadius)
            {
                _current.Destination.AddRace(_passenger);
                _passenger = null;
                _current = null;
            }
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

            if (_current != null)
            {
                ordersToSave.Insert(0, _current);//TODO: can be optimized
            }
            
            writer.Write(ordersToSave.Count);

            foreach (var order in ordersToSave)
            {
                writer.Write(order.Race.SaveIndex);
                writer.Write(order.Departure.SaveIndex);
                writer.Write(order.Destination.SaveIndex);
            }

            writer.Write(_current != null);
        }

        public override void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            transform.localRotation = reader.ReadQuaternion();

            _current = null;
            _passenger = null;
            _ordersQueue.Clear();
            var count = reader.ReadInt();
            
            for (int i = 0; i < count; i++)
            {
                var race = GameController.Instance.Races[reader.ReadInt()];
                var departure = GameController.Instance.Planets[reader.ReadInt()];
                var destination = GameController.Instance.Planets[reader.ReadInt()];
                _ordersQueue.Enqueue(new Order(race, departure, destination));
                
                if (race.PlanetSaveIndex >= 0)
                {
                    departure.AddRaceToDeparture(race);
                }
                destination.AddRaceToArrive(race);
            }

            if (reader.ReadBool())
            {
                _current = _ordersQueue.Dequeue();
                if (_current.Race.PlanetSaveIndex < 0)
                {
                    _passenger = _current.Race;
                }
            }
        }
    }
}