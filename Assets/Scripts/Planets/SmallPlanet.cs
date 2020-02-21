using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SmallPlanet : Planet
    {
        public override bool IsFull { get; }
        public override bool IsEmpty { get; }
        public override void Veil()
        {
            throw new System.NotImplementedException();
        }

        public override void Unveil()
        {
            throw new System.NotImplementedException();
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            throw new System.NotImplementedException();
        }

        public override bool AddRace(Race race)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveRace(Race race)
        {
            throw new System.NotImplementedException();
        }
    }
}