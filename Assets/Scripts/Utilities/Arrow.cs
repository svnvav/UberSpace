using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private SpriteRenderer _head;

        public void SetPosition(int index, Vector3 position)
        {
            _line.SetPosition(index, position);
            if (_line.positionCount > 1)
            {
                var from = _line.GetPosition(_line.positionCount - 2);
                var to = _line.GetPosition(_line.positionCount - 1);
                
                _head.transform.position = to;
                _head.transform.up = to - from;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _line.positionCount; i++)
            {
                _line.SetPosition(i, Vector3.zero);
            }
        }
    }
}