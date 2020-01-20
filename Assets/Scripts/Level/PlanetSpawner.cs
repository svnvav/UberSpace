using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : GameLevelObject
    {
        public PlanetFactory Factory;

        [SerializeField, Range(0f, 64f)] private float _spawnSpeed;
        [SerializeField, Range(0f, 64f)] private float _planetSpeed;
        
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
            var planet = Factory.Get();
            planet.transform.position = transform.position;
            planet.Initialize(-_planetSpeed * transform.position.normalized);
        }

        public override void Save (GameDataWriter writer) {
            writer.Write(_spawnProgress);
        }

        public override void Load (GameDataReader reader) {
            _spawnProgress = reader.ReadFloat();
        }
    }
}