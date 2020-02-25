using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpriteRendererVeil
    {
        private Color _veilColor;
        private List<SpriteRenderer> _renderers;
        private List<Color> _originalColors;
        private bool _veiled;

        public bool Veiled => _veiled;

        public SpriteRendererVeil(Color veilColor)
        {
            _veilColor = veilColor;
            _renderers = new List<SpriteRenderer>();
            _originalColors = new List<Color>();
        }

        public void AddSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            _renderers.Add(spriteRenderer);
            _originalColors.Add(spriteRenderer.color);
            if (_veiled)
            {
                spriteRenderer.color *= _veilColor;
            }
        }

        public void RemoveSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            var index = _renderers.IndexOf(spriteRenderer);
            while (index != -1)
            {
                _renderers[index].color = _originalColors[index];
                _renderers.RemoveAt(index);
                _originalColors.RemoveAt(index);
                index = _renderers.IndexOf(spriteRenderer);
            }
            
        }

        public void Veil()
        {
            for (int i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].color *= _veilColor;
            }

            _veiled = true;
        }

        public void Unveil()
        {
            for (int i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].color = _originalColors[i];
            }

            _veiled = false;
        }
    }
}