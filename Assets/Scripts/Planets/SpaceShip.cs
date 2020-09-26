using System;
using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpaceShip : Planet
    {
        [SerializeField] private List<Race> _races;
        [SerializeField] private int _capacity;

        [SerializeField] private SpriteRenderer _defaultSprite;
        
        private List<Race> _racesToArrive, _racesToDeparture;

        public override int Capacity => _capacity;

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
        public override bool IsFull => _races.Count + _racesToArrive.Count >= Capacity;
        public override bool IsEmpty => _races.Count + _racesToArrive.Count == 0 || _racesToDeparture.Count == _races.Count;

        private void Awake()
        {
            _racesToArrive = new List<Race>();
            _racesToDeparture = new List<Race>();
            
            base.Awake();
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            return _races[0];
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
            throw new System.NotImplementedException();
        }

        public override void RemoveRaceToDeparture(Race race)
        {
            throw new System.NotImplementedException();
        }

        private void RefreshView()
        {
            
        }
    }
}