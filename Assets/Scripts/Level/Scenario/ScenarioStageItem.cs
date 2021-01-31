using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [Serializable]
    public class ScenarioStageItem
    {
        public Spawner Spawner;

        public float ActivationDelay;
        public int QuantityLimit;
        [SerializeField, Range(0.1f, 64f)] private float _spawnCooldown;
        [SerializeField, Range(0.1f, 64f)] private float _planetSpeed;
        
        private bool _activated;
        private float _progress;
        private int _count;
        
        private bool LimitExceeded => QuantityLimit > 0 && _count >= QuantityLimit;

        public void Progress(float deltaTime, bool last = false)
        {
            _progress += deltaTime;

            if (!_activated)
            {
                if (_progress > ActivationDelay)
                {
                    Spawner.Spawn(_planetSpeed);
                    _count++;
                    _activated = true;
                    _progress -= ActivationDelay;
                }
                else
                {
                    return;
                }
            }
            
            while (!LimitExceeded && _progress > _spawnCooldown)
            {
                _progress -= _spawnCooldown;
                Spawner.Spawn(_planetSpeed);
                _count++;
            }
        }

        public void SetTime(float time)
        {
            _count = 0;
            _progress = time * _spawnCooldown;
            _activated = _progress > ActivationDelay;
            if (_activated)
            {
                _progress -= ActivationDelay;
                _count = Mathf.FloorToInt(_progress);
                _count = Mathf.Max(_count, QuantityLimit);
                _progress -= _count;
            }
        }
    }
}