using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class Planet : MonoBehaviour, IPersistable, IGameUpdatable
    {
        [Header("Base Planet")]
        [SerializeField] protected Vector3 _velocity;
        [SerializeField] private Color _veilColor;
        [SerializeField] private ParticleSystem _raceEffect;
        protected SpriteRendererVeil _veil;

        protected bool _veiling;

        //public Action<Planet> OnDie;//TODO: possible memory leaks
        
        public abstract float Radius { get; }

        public abstract int SaveIndex { get; set; }//index in GameController._planets
        
        public abstract int Capacity { get; }
        public abstract bool IsFull { get; }
        public abstract bool IsEmpty { get; }//TODO: change name or departure logic, fix arriving race departure
        
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

        protected void Awake()
        {
            _veil = new SpriteRendererVeil(_veilColor);
        }

        public virtual void Initialize(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public virtual void GameUpdate(float deltaTime)
        {
            transform.Translate(deltaTime * _velocity);
        }
        
        public Vector3 OnTheEdgeFrom(Vector3 from)
        {
            var t = 1f - Radius / (from - transform.position).magnitude;

            return Vector3.Lerp(from, transform.position, t);
        }

        public void SetVeiling(bool value)
        {
            _veiling = value;
            if (!_veiling)
            {
                _veil.Unveil();
            }
            else if(IsFull)
            {
                _veil.Veil();
            }
        }

        public void Poof(Race race)
        {
            _raceEffect.textureSheetAnimation.SetSprite(0, race.PlanetSprite);
            _raceEffect.Play();
        }
        
        public abstract Race GetRaceByTouchPos(Vector3 touchPos);

        public virtual void AddRaceToArrive(Race race)
        {
            if (IsFull)
            {
                Debug.LogError("Trying to put a race to full planet");//TODO: Debug for big Planet
            }
        }

        public abstract void AddRace(Race race, bool hard = false);
        
        public virtual void AddRaceToDeparture(Race race)
        {
            if (IsEmpty)
            {
                Debug.LogError("There is no race to departure");
            }
        }
        public abstract void DepartureRace(Race race);
        
        public abstract void RemoveRaceToArrive(Race race);
        
        public abstract void RemoveRaceToDeparture(Race race);

        public virtual void Die()
        {
            //OnDie?.Invoke(this);
            //OnDie = null;
            GameController.Instance.RemovePlanet(this);//TODO: remove when OnDie implemented
            Recycle();
        }

        public virtual void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        public virtual void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(_velocity);
        }

        public virtual void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            _velocity = reader.ReadVector3();
        }
    }
}