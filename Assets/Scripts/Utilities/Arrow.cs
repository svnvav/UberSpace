using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private SpriteRenderer _head;
        [SerializeField] private float _arrowHeadLineOffset;

        public void SetPosition(int index, Vector3 position)
        {
            _line.SetPosition(index, position);
            if (_line.positionCount > 1)
            {
                var from = _line.GetPosition(_line.positionCount - 2);
                var to = _line.GetPosition(_line.positionCount - 1);
                var t = 1f - _arrowHeadLineOffset / (from - to).magnitude;
                
                _line.SetPosition(_line.positionCount - 1, Vector3.Lerp(from, to, t));
                
                _head.gameObject.SetActive(true);
                _head.transform.position = to;
                _head.transform.up = to - from;
            }
            else
            {
                _head.gameObject.SetActive(false);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _line.positionCount; i++)
            {
                _line.SetPosition(i, Vector3.zero);
            }
            _head.gameObject.SetActive(false);
        }
    }
}