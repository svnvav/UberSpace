using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class LevelStageMenuItem : MonoBehaviour
    {
        private MainMenuController _controller;
        private string _saveFileName;
        private bool _initialized;

        public void Initialize(MainMenuController controller, string saveFileName)
        {
            _controller = controller;
            _saveFileName = saveFileName;
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