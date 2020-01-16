using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Planet : PersistableObject
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private RaceOnPlanet _leftRace, _rightRace;
        [SerializeField] private GameObject _leftRaceSprite, _rightRaceSprite;

        #region Factory
        
        private int _idInFactory = int.MinValue;

        private PlanetFactory _originFactory;

        public int IdInFactory {
            get => _idInFactory;
            set {
                if (_idInFactory == int.MinValue) {
                    _idInFactory = value;
                }
                else {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }
        
        public PlanetFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                if (_originFactory == null) {
                    _originFactory = value;
                }
                else {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }
        
        #endregion

        public void Initialize(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void GameUpdate()
        {
            transform.Translate(Time.deltaTime * _velocity);
            //TODO: races war
        }

        public bool AddRace(RaceOnPlanet raceOnPlanet)
        {
            if (_leftRace == null)
            {
                _leftRace = raceOnPlanet;
                RefreshView();
                return true;
            }
            
            if (_rightRace == null)
            {
                _rightRace = raceOnPlanet;
                RefreshView();
                return true;
            }

            return false;
        }

        private void RefreshView()
        {
            _leftRaceSprite.SetActive(_leftRace != null);
        }
        
        public void Recycle ()
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