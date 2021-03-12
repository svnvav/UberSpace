﻿using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelPreviewComet : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        private float _initialTransparency;

        private void Awake()
        {
            _initialTransparency = _fill.color.a;
        }

        public void SetTransparency(float value)
        {
            var color = _fill.color;
            color.a = value * _initialTransparency;
            _fill.color = color;
        }
    }
}