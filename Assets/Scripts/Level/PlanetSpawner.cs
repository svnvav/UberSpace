using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PlanetSpawner : Spawner
    {
        public PlanetFactory Factory;

        [SerializeField] private float _activationDelay;
        [SerializeField] private int _count;
        [SerializeField, Range(0f, 64f)] private float _spawnSpeed;
        
        [SerializeField, Range(0f, 64f)] private float _planetSpeed;

        private bool _activated;
        
        private float _spawnProgress;

        public override void Progress(float deltaTime)
        {
            _spawnProgress += deltaTime * _spawnSpeed;

            if (!_activated)
            {
                if (_spawnProgress >= _activationDelay)
                {
                    _activated = true;
                    _spawnProgress -= _activationDelay;
                }
                else
                {
                    return;
                }
            }

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

        
    }
}