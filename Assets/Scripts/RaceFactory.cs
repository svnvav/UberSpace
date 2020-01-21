using UnityEngine;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class RaceFactory : ScriptableObject
    {
        [SerializeField] private RaceInstance _prefab;
        [SerializeField] private RaceInfo[] _infos;

        public RaceInfo[] Infos => _infos;
    }
}