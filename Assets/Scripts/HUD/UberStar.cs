using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UberStar : MonoBehaviour
    {
        [SerializeField] private Sprite _spriteForEmpty;
        [SerializeField] private Sprite _spriteForFilled;
        [SerializeField] private bool _isFilled;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateView();
        }

        public void SetFilled(bool value)
        {
            _isFilled = value;
            UpdateView();
        }

        private void UpdateView()
        {
            _spriteRenderer.sprite = _isFilled ? _spriteForFilled : _spriteForEmpty;
        }
    }
}