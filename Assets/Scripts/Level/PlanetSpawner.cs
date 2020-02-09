using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : Spawner
    {
        public PlanetFactory Factory;
        
        [SerializeField, Range(0f, 64f)] private float _planetSpeed;
        
        public override void Spawn()
        {
            var planet = Factory.Get();
            planet.transform.position = transform.position;
            planet.Initialize(-_planetSpeed * transform.position.normalized);
        }

    }
}