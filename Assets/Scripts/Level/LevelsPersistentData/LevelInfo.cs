using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [Serializable]
    public class LevelInfo
    {
        public string Name => _name;

        public string SceneName => _sceneName;

        public LevelPreview Preview => _preview;

        public Color Color => _color;

        public int StarsCount => _starsCount;

        public Sprite[] LevelStageSprites => _levelStageSprites;
        
        [SerializeField] private string _name;
        [SerializeField] private string _sceneName;
        [SerializeField] private LevelPreview _preview;
        [SerializeField] private Color _color;
        [SerializeField] private int _starsCount;
        [SerializeField] private Sprite[] _levelStageSprites;
    }
}