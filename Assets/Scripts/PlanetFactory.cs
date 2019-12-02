using UnityEngine;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class PlanetFactory : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
    }
}