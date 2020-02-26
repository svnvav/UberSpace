using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Order
    {
        private Race _race;
        private Planet _departure, _destination;
        private OrderStatus _status;

        public Race Race => _race;
        public Planet Departure => _departure;
        public Planet Destination => _destination;
        public OrderStatus Status => _status;

        public Order()
        {
            _status = OrderStatus.Completed;
        }

        public void Init(Race race, Planet departure, Planet destination, OrderStatus status)
        {
            _race = race;
            _departure = departure;
            _destination = destination;

            SetStatus(status);
        }

        public void SetStatus(OrderStatus status)
        {
            _status = status;

            switch (status)
            {
                case OrderStatus.Completed:
                    _race = null;
                    _departure = null;
                    _destination = null;
                    break;
                case OrderStatus.Queued:
                case OrderStatus.Taken:
                    _departure.AddRaceToDeparture(_race);
                    _destination.AddRaceToArrive(_race);
                    break;
                case OrderStatus.Executing:
                    _destination.AddRaceToArrive(_race);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
        
        public Vector3 GetCurrentPointToMove()
        {
            switch (_status)
            {
                case OrderStatus.Taken:
                    return _departure.transform.position;
                case OrderStatus.Executing:
                    return _destination.transform.position;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Take()
        {
            _status = OrderStatus.Taken;
        }

        public void StartExecuting()
        {
            _departure.DepartureRace(_race);
            _race.PlanetSaveIndex = -_race.PlanetSaveIndex - 1;
            
            _status = OrderStatus.Executing;
        }
        
        public void Complete()
        {
            _destination.AddRace(_race);

            SetStatus(OrderStatus.Completed);
        }

        public void Cancel()
        {
            switch (_status)
            {
                case OrderStatus.Queued:
                case OrderStatus.Taken:
                    _departure.RemoveRaceToDeparture(_race);
                    _destination.RemoveRaceToArrive(_race);
                    SetStatus(OrderStatus.Completed);
                    break;
                case OrderStatus.Executing:
                    _destination = _departure;
                    _destination.AddRaceToArrive(_race);
                    break;
            }
        }
    }
}