using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpaceShipSpawner : Spawner
    {
        [SerializeField, Range(0f, 64f)] private float _planetSpeed;

        [SerializeField] private int _planetIndex;

        [SerializeField] private int _raceIndex = -1;
        
        public override void Spawn()
        {
            var spaceShip = GameController.Instance.PlanetFactory.Get(_planetIndex);
            spaceShip.transform.position = transform.position;
            spaceShip.transform.up = transform.up;
            spaceShip.Initialize(new Vector2(_planetSpeed, _planetSpeed));
            if (_raceIndex > -1)
            {
                var race = GameController.Instance.RaceFactory.Get(_raceIndex);
                spaceShip.AddRace(race, true);
            }
        }
    }
}