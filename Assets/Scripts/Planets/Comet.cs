using System.Collections;
using Catlike.ObjectManagement;
using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Comet : MonoBehaviour, IPersistable
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private Animator _animator;

        private int _deathAnimHash = Animator.StringToHash("Death");
        
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
            transform.Translate(Time.deltaTime * _velocity);
        }

        private void OnMouseDown()
        {
            if (_touched) return;
            _touched = true;

            CoreSceneController.Instance.CoreDataProvider.OnCometCaught(_id);
            
            _animator.Play(_deathAnimHash);
        }

        public void SwitchOff()
        {
            gameObject.SetActive(false);
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