using System;
using System.Collections;
using Catlike.ObjectManagement;
using Svnvav.UberSpace.CoreScene;
using UnityEditorInternal;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Comet : MonoBehaviour, IPersistable
    {
        [SerializeField] private float _touchRadius;
        [SerializeField] private Vector3 _velocity;

        private int _id;
        private bool _touched;

        public void Initialize(int id, Vector3 velocity)
        {
            _id = id;
            _velocity = velocity;
            _touched = false;
        }
        
        private void Update()
        {
            /*if (Input.GetMouseButtonDown(0))
            {
                var touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPos.z = 0;
                if ((transform.position - touchPos).sqrMagnitude < _touchRadius * _touchRadius)
                {
                    StartCoroutine(OnMouseDown());
                }
            }*/

            transform.Translate(Time.deltaTime * _velocity);
        }

        private IEnumerator OnMouseDown()
        {
            if (_touched) yield break;
            _touched = true;

            CoreSceneController.Instance.CoreDataProvider.OnCometCaught(_id);
            
            gameObject.SetActive(false);//TODO: animation event
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