using System;
using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelStageMenuItem : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _image;
        
        public string SaveFileName => _saveFileName;

        private string _saveFileName;
        private bool _initialized;
        private Action _onClick;

        public void Initialize(string saveFileName, Sprite sprite, Action onClick)
        {
            _saveFileName = saveFileName;
            _image.sprite = sprite;
            _onClick = onClick;
            
            _initialized = true;
        }

        public void OnClick()
        {
            if (!_initialized)
            {
                throw new Exception("Not initialized!");
            }
            _onClick.Invoke();
        }

        public void Dispose()
        {
            _onClick = null;
        }
    }
}