
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : Spawner
    {
        [SerializeField] private int _planetIndex;

        [SerializeField] private int _raceIndex = -1;
        
        public override void Spawn(float speed)
        {
            var planet = GameController.Instance.PlanetFactory.Get(_planetIndex);
            planet.transform.position = transform.position;
            planet.Initialize(-speed * transform.position.normalized);
            if (_raceIndex > -1)
            {
                var race = GameController.Instance.RaceFactory.Get(_raceIndex);
                planet.AddRace(race, true);
            }
        }
    }
}