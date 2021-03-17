using System.Linq;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpaceObjectPath : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform[] _points;

#if UNITY_EDITOR
        [SerializeField] private bool _debug;
#endif

        private Vector3[] _positions;
        
        public Transform SpawnPoint => _spawnPoint;

        private void Start()
        {
            _positions = _points.Select(point => point.position).ToArray();
        }

        public Vector3 GetPathPoint(float t)
        {
            return Vector3.Lerp(_positions[0], _positions[1], t);
            //stub TODO:
        }
    }
}