using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Planet : PersistableObject
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private List<Race> _races;
        [SerializeField] private SpriteRenderer _leftRaceSprite, _rightRaceSprite;
        [SerializeField] private GameObject _spriteMask;

        public int SaveIndex { get; set; }//index in GameController._planets
        
        public int RacesCount => _races.Count;
        
        #region Factory

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

        private void Start()
        {
            //GameController.Instance.AddPlanet(this);
        }

        private void OnEnable()
        {
            RefreshView();
        }

        public void Initialize(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void GameUpdate()
        {
            transform.Translate(Time.deltaTime * _velocity);
            //TODO: races war
        }

        public void Die()
        {
            GameController.Instance.RemovePlanet(this);
            foreach (var race in _races)
            {
                race.Die();
            }
            Recycle();
        }

        public Race GetRaceById(int id)
        {
            return _races[id];
        }
        
        public bool AddRace(Race race)
        {
            if (_races.Count >= 2) return false;
            
            _races.Add(race);
            race.PlanetSaveIndex = SaveIndex;
            RefreshView();
            return true;
        }

        public void RemoveRace(Race race)
        {
            _races.Remove(race);
            RefreshView();
        }

        private void RefreshView()
        {
            var racesCount = _races.Count;
            _leftRaceSprite.gameObject.SetActive(racesCount > 0);
            _leftRaceSprite.sprite = racesCount > 0 ? _races[0].PlanetSprite : null;

            _rightRaceSprite.gameObject.SetActive(racesCount > 1);
            _rightRaceSprite.sprite = racesCount > 1 ? _races[1].PlanetSprite : null;
            _spriteMask.SetActive(racesCount > 1);
        }

        public void Recycle()
        {
            _races.Clear();
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
            RefreshView();
        }
    }
}