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

        public void Init(Race race, Planet departure, Planet destination)
        {
            _race = race;
            _departure = departure;
            _destination = destination;
            
            departure.AddRaceToDeparture(race);
            destination.AddRaceToArrive(race);
            
            _status = OrderStatus.Queued;
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
        }
        
        public void Complete()
        {
            _destination.AddRace(_race);
            _race = null;
            _departure = null;
            _destination = null;

            _status = OrderStatus.Completed;
        }

        public void Cancel(Action onExecutionCancel)
        {
            switch (_status)
            {
                case OrderStatus.Queued:
                case OrderStatus.Taken:
                    _departure.RemoveRaceToDeparture(_race);
                    _destination.RemoveRaceToArrive(_race);
                    break;
                case OrderStatus.Executing:
                    onExecutionCancel();
                    break;
            }
            _race = null;
            _departure = null;
            _destination = null;

            _status = OrderStatus.Completed;
        }
    }
}