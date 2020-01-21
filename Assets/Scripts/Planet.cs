using System;
using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Planet : PersistableObject
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private List<RaceInstance> _races;
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
            Recycle();
        }

        public RaceInstance GetRaceById(int id)
        {
            return _races[id];
        }
        
        public bool AddRace(RaceInstance raceInstance)
        {
            if (_races.Count >= 2) return false;
            
            _races.Add(raceInstance);
            RefreshView();
            return true;
        }

        public void RemoveRace(RaceInstance raceInstance)
        {
            _races.Remove(raceInstance);
            RefreshView();
        }

        private void RefreshView()
        {
            var racesCount = _races.Count;
            _leftRaceSprite.gameObject.SetActive(racesCount > 0);
            _leftRaceSprite.sprite = racesCount > 0 ? _races[0].Info.PlanetSprite : null;

            _rightRaceSprite.gameObject.SetActive(racesCount > 1);
            _rightRaceSprite.sprite = racesCount > 1 ? _races[1].Info.PlanetSprite : null;
            _spriteMask.SetActive(racesCount > 1);
        }

        public void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(_velocity);
            writer.Write(RacesCount);
            foreach (var race in _races)
            {
                race.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVector3();
            _velocity = reader.ReadVector3();
            var racesCount = reader.ReadInt();
            _races.Clear();
            for (int i = 0; i < racesCount; i++)
            {
                //_races.Add();
            }

        }
    }
}