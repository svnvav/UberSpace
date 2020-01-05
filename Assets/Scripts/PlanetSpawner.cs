using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : GameLevelObject
    {
        public PlanetFactory Factory;

        [SerializeField, Range(0f, 50f)] float _spawnSpeed;
        
        private float _spawnProgress;

        public override void GameUpdate()
        {
            _spawnProgress += Time.deltaTime * _spawnSpeed;
            while (_spawnProgress >= 1f)
            {
                _spawnProgress -= 1f;
                Spawn();
            }
        }
        
        private void Spawn()
        {
            var planet = Factory.Get(transform.position);
            planet.Initialize(-transform.position.normalized);
            
        }
    }
}