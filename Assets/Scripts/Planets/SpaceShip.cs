using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpaceShip : Planet
    {
        [SerializeField] private int _capacity;

        public override int Capacity => _capacity;

        private List<Race> _racesOnBoard;

        public override float Radius { get; }
        public override int SaveIndex { get; set; }
        public override bool IsFull { get; }
        public override bool IsEmpty { get; }
        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            throw new System.NotImplementedException();
        }

        public override void AddRace(Race race, bool hard = false)
        {
            throw new System.NotImplementedException();
        }

        public override void DepartureRace(Race race)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveRaceToArrive(Race race)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveRaceToDeparture(Race race)
        {
            throw new System.NotImplementedException();
        }
    }
}