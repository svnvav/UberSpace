using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private Queue<(Race, Planet, Planet)> _queue;

        private Race _currentPassenger;

        public void AddOrder(Race passenger, Planet from, Planet to)
        {
            _queue.Enqueue((passenger, from, to));
        }

        private void Update()
        {
            //TODO:
        }
    }
}