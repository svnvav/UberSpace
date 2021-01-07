using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [Serializable]
    public class LevelInfo
    {
        public string Name => _name;

        public string SceneName => _sceneName;

        public Sprite Preview => _preview;

        [SerializeField] private string _name;
        [SerializeField] private string _sceneName;
        [SerializeField] private Sprite _preview;
    }
}