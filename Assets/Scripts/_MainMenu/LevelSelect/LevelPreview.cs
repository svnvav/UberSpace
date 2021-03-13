using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class LevelPreview : MonoBehaviour
    {
        [SerializeField] private LevelPreviewComet[] _cometSlots;

        public void SetTransparency(float value)
        {
            foreach (var levelPreviewComet in _cometSlots)
            {
                levelPreviewComet.SetTransparency(value);
            }
        }
    }
}