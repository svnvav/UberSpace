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
        [Range(0f, 64f)] public float Speed;
        
        private bool _activated;
        private float _progress;
        private int _count;
        
        private bool LimitExceeded => QuantityLimit > 0 && _count >= QuantityLimit;

        public void Progress(float deltaTime, bool last = false)
        {
            _progress += deltaTime * Speed;

            if (!_activated)
            {
                if (_progress > ActivationDelay * Speed)
                {
                    _activated = true;
                    _progress -= ActivationDelay * Speed;
                }
                else
                {
                    return;
                }
            }
            
            while (!LimitExceeded && _progress > 1f)
            {
                _progress -= 1f;
                Spawner.Spawn();
                _count++;
            }
        }

        public void SetTime(float time)
        {
            _count = 0;
            _progress = time * Speed;
            _activated = _progress > ActivationDelay * Speed;
            if (_activated)
            {
                _progress -= ActivationDelay * Speed;
                _count = Mathf.FloorToInt(_progress);
                _count = Mathf.Max(_count, QuantityLimit);
                _progress -= _count;
            }
        }
    }
}