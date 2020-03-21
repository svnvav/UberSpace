using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public sealed class BigPlanet : Planet
    {
        [SerializeField] private List<Race> _races;
        [SerializeField] private SpriteRenderer _defaultSprite, _leftRaceSprite, _rightRaceSprite, _leftRaceToArriveSprite, _rightRaceToArriveSprite;
        [SerializeField] private GameObject _rightRaceMask, _rightRaceToArriveMask;

        private List<Race> _racesToArrive, _racesToDeparture;
        
        private int _saveIndex;
        public override int SaveIndex
        {
            get => _saveIndex;
            set
            {
                _saveIndex = value;
                foreach (var race in _races)
                {
                    race.PlanetSaveIndex = _saveIndex;
                }
            }
        }

        public override float Radius => 0.5f * _defaultSprite.transform.lossyScale.x;

        public override int Capacity => 2;
        public override bool IsFull => _races.Count + _racesToArrive.Count >= Capacity;
        public override bool IsEmpty => _races.Count + _racesToArrive.Count == 0 || _racesToDeparture.Count == _races.Count;

        protected new void Awake()
        {
            _racesToArrive = new List<Race>();
            _racesToDeparture = new List<Race>();
            
            base.Awake();
            _veil.AddSpriteRenderer(_defaultSprite);
            _veil.AddSpriteRenderer(_leftRaceSprite);
            _veil.AddSpriteRenderer(_rightRaceSprite);
            _veil.AddSpriteRenderer(_leftRaceToArriveSprite);
            _veil.AddSpriteRenderer(_rightRaceToArriveSprite);
        }

        private void OnEnable()
        {
            RefreshView();
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            var id = _races.Count == 1 ? 
                0 : 
                (touchPos.x - transform.position.x < 0 ? 0 : 1);//0 for left and 1 for right
            
            return _races[id];
        }

        public override void AddRaceToArrive(Race race)
        {
            base.AddRaceToArrive(race);
            _racesToArrive.Add(race);
            RefreshView();
        }
        
        public override void AddRace(Race race, bool hard = false)
        {
            if (!_racesToArrive.Contains(race) && !hard)
            {
                Debug.LogError("RaceToArrive and actual race are different");
            }

            _racesToArrive.Remove(race);
            _races.Add(race);
            race.PlanetSaveIndex = SaveIndex;
            RefreshView();
        }

        public override void AddRaceToDeparture(Race race)
        {
            if (!_races.Contains(race))
            {
                Debug.LogError("There is no that RaceToDeparture");
            }
            _racesToDeparture.Add(race);
        }

        public override void DepartureRace(Race race)
        {
            if (!_races.Contains(race))
            {
                Debug.LogError("There is no that race to departure");
            }
            if (!_racesToDeparture.Contains(race))
            {
                Debug.LogError("RaceToDeparture and actual race are different");
            }
            
            _races.Remove(race);
            _racesToDeparture.Remove(race);
            
            RefreshView();
        }

        public override void RemoveRaceToArrive(Race race)
        {
            if (!_racesToArrive.Contains(race))
            {
                Debug.LogError("_racesToArrive list does not contains that race");
            }
            _racesToArrive.Remove(race);
            RefreshView();
        }

        public override void RemoveRaceToDeparture(Race race)
        {
            if (!_racesToDeparture.Contains(race))
            {
                Debug.LogError("_racesToDeparture list does not contains that race");
            }
            _racesToDeparture.Remove(race);
        }

        private void RefreshView()
        {
            var racesCount = _races.Count;
            
            _defaultSprite.gameObject.SetActive(racesCount == 0);
            
            _leftRaceSprite.gameObject.SetActive(racesCount > 0);
            _leftRaceSprite.sprite = racesCount > 0 ? _races[0].PlanetSprite : null;

            _rightRaceSprite.gameObject.SetActive(racesCount > 1);
            _rightRaceSprite.sprite = racesCount > 1 ? _races[1].PlanetSprite : null;
            _rightRaceMask.SetActive(racesCount > 1);
            
            var racesToArriveCount = _racesToArrive.Count;

            if (racesCount == 0)
            {
                _leftRaceToArriveSprite.gameObject.SetActive(racesToArriveCount > 0);
                _leftRaceToArriveSprite.sprite = racesToArriveCount > 0 ? _racesToArrive[0].PlanetSprite : null;

                _rightRaceToArriveSprite.gameObject.SetActive(racesToArriveCount > 1);
                _rightRaceToArriveSprite.sprite = racesToArriveCount > 1 ? _racesToArrive[1].PlanetSprite : null;
                _rightRaceToArriveMask.SetActive(racesToArriveCount > 1);
            }
            else
            {
                _rightRaceToArriveMask.SetActive(racesToArriveCount > 0);
                
                _rightRaceToArriveSprite.gameObject.SetActive(racesToArriveCount > 0);
                _rightRaceToArriveSprite.sprite = racesToArriveCount > 0 ? _racesToArrive[0].PlanetSprite : null;
                
                _leftRaceToArriveSprite.gameObject.SetActive(racesToArriveCount > 1);
                _leftRaceToArriveSprite.sprite = racesToArriveCount > 1 ? _racesToArrive[1].PlanetSprite : null;
            }

            if (_veiling && IsFull && !_veil.Veiled)
            {
                _veil.Veil();
            }
            else
            {
                _veil.Unveil();
            }
        }

        
        public override void Die()
        {
            foreach (var race in _races)
            {
                race.Die();
            }
            base.Die();
        }
        public override void Recycle()
        {
            base.Recycle();
            _races.Clear();
            _racesToArrive.Clear();
            _racesToDeparture.Clear();
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
    }
}