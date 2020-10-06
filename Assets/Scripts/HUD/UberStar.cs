using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [RequireComponent(typeof(Animator))]
    public class UberStar : MonoBehaviour
    {
        [SerializeField] private Animator _starAnimator;
        [SerializeField] private bool _isFilled;

        private void Awake()
        {
            UpdateView();
        }

        public void SetFilled(bool value)
        {
            _isFilled = value;
            UpdateView();
        }

        private void UpdateView()
        {
            _starAnimator.Play(_isFilled ? "Filled" : "Empty");
        }
    }
}