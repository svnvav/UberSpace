using UnityEngine;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class LevelsInfoSO : ScriptableObject
    {
        public LevelInfo[] Levels => _levels;

        [SerializeField] private LevelInfo[] _levels;
    }
}