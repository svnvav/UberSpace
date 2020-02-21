
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : Spawner
    {
        [SerializeField, Range(0f, 64f)] private float _planetSpeed;

        [SerializeField] private int _planetIndex;

        [SerializeField] private int _raceIndex = -1;
        
        public override void Spawn()
        {
            var planet = GameController.Instance.PlanetFactory.Get(_planetIndex);
            planet.transform.position = transform.position;
            planet.Initialize(-_planetSpeed * transform.position.normalized);
            if (_raceIndex > -1)
            {
                var race = GameController.Instance.RaceFactory.Get(_raceIndex);
                planet.AddRace(race);
            }
        }
    }
}