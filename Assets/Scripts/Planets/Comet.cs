using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Comet : MonoBehaviour, IPersistable
    {
        [SerializeField] private float _touchRadius;
        [SerializeField] private Vector3 _velocity;

        private int _id;

        public void Initialize(int id, Vector3 velocity)
        {
            _id = id;
            _velocity = velocity;
        }
        
        private void Update()
        {
            var touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if ((transform.position - touchPos).sqrMagnitude < _touchRadius * _touchRadius)
            {
                Debug.LogWarning("Comet touched!");
            }
            
            transform.Translate(Time.deltaTime * _velocity);//TODO:
        }

        public void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(_velocity);
        }

        public void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            _velocity = reader.ReadVector3();
        }

        
    }
}