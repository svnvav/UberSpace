using System;
using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace
{
    public class LevelStageMenuItem : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
        private MainMenuController _controller;
        private string _saveFileName;
        private bool _initialized;

        public void Initialize(MainMenuController controller, string saveFileName)
        {
            _controller = controller;
            _saveFileName = saveFileName;
            _text.text = _saveFileName;//TODO: change to normal name
            _initialized = true;
        }

        public void OnClick()
        {
            if (!_initialized)
            {
                throw new Exception("Not initialized!");
            }
            _controller.LoadLevel(_saveFileName);
        }
    }
}