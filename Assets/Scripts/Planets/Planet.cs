using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class Planet : PersistableObject
    {
        [SerializeField] protected Vector3 _velocity;
        
        public int SaveIndex { get; set; }//index in GameController._planets
        
        public abstract int Capacity { get; }
        public abstract bool IsFull { get; }
        public abstract bool IsEmpty { get; }
        
        #region Factory

        private int _prefabId = int.MinValue;
        public int PrefabId
        {
            get => _prefabId;
            set
            {
                if (_prefabId == int.MinValue)
                {
                    _prefabId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }
        
        private PlanetFactory _originFactory;
        public PlanetFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                if (_originFactory == null)
                {
                    _originFactory = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }

        #endregion

        public void Initialize(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public virtual void GameUpdate(float deltaTime)
        {
            transform.Translate(deltaTime * _velocity);
        }

        public abstract void Veil();
        public abstract void Unveil();

        public abstract Race GetRaceByTouchPos(Vector3 touchPos);

        public abstract bool AddRace(Race race);
        public abstract void RemoveRace(Race race);

        public virtual void Die()
        {
            GameController.Instance.RemovePlanet(this);
            Recycle();
        }

        public virtual void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(_velocity);
        }

        public override void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            _velocity = reader.ReadVector3();
        }
    }
}