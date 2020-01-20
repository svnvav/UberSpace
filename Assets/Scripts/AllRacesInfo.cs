using UnityEngine;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class AllRacesInfo : ScriptableObject
    {
        [SerializeField] private RaceInfo[] _infos;

        public RaceInfo[] Infos => _infos;
    }
}