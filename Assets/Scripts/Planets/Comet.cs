using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Comet : MonoBehaviour, IPersistable, IGameUpdatable
    {
        [SerializeField] private Vector3 _velocity;

        private int _id;

        public void Initialize(int id, Vector3 velocity)
        {
            _id = id;
            _velocity = velocity;
        }
        
        public void GameUpdate(float deltaTime)
        {
            transform.Translate(deltaTime * _velocity);//TODO:
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