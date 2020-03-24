using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class TaxiService : PersistableObject
    {
        [SerializeField] private Taxi _taxi;
        
        private OrderLinkedQueue _queue; //TODO: delete dead races, if the reason is not a planet death
        private OrderPool _pool;

        private void Awake()
        {
            _queue = new OrderLinkedQueue();
            _pool = new OrderPool(1);
        }

        private void Start()
        {
            GameController.Instance.OnRemovePlanet.RegisterCallback(RemoveOrdersWithPlanet);
        }

        public void GameUpdate(float deltaTime)
        {
            foreach (var order in _queue)
            {
                order.GameUpdate();
            }
            
            if (_taxi.IsIdle && _queue.Count > 0)
            {
                _taxi.ExecuteOrder(_queue.Peek(), OnOrderCompleted);
            }
            
            _taxi.GameUpdate(deltaTime);
        }

        public void AddOrder(Race passenger, Planet departure, Planet destination)
        {
            _queue.Enqueue(_pool.Get(passenger, departure, destination));
        }

        private void OnOrderCompleted()
        {
            _queue.Dequeue();
            _taxi.ExecuteOrder(_queue.Peek(), OnOrderCompleted);
        }
        
        private void RemoveOrdersWithPlanet(Planet planet)
        {
            //TODO: consider dragndrop from taxiship
            if (_queue.Count == 0) return;

            Order canceled = null;

            var current = _queue.Peek();
            if (current.Status == OrderStatus.Executing)
            {
                if (current.Destination == planet)
                {
                    canceled = _queue.RemoveAndGetLast(o => o.Destination == current.Departure);
                    canceled?.Cancel();
                    //return back
                    current.Cancel();
                }
            }

            foreach (var order in _queue)
            {
                if (order.Status != OrderStatus.Executing && 
                    (order.Departure == planet || order.Destination == planet)
                )
                {
                    order.Cancel();
                }
            }
            
            _queue.RemoveAll(order => order.Status == OrderStatus.Completed);
        }

        public override void Save(GameDataWriter writer)
        {
            _taxi.Save(writer);
            
            var ordersToSave = new List<Order>(_queue);

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
            _taxi.Load(reader);
            
            _queue.Clear();
            var count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                var race = GameController.Instance.Races[reader.ReadInt()];
                var departure = GameController.Instance.Planets[reader.ReadInt()];
                var destination = GameController.Instance.Planets[reader.ReadInt()];
                var status = (OrderStatus) reader.ReadInt();

                _queue.Enqueue(_pool.Get(race, departure, destination, status));
            }
        }
    }
}