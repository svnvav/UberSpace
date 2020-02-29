using System;
using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public sealed class BigPlanet : Planet
    {
        [SerializeField] private List<Race> _races;
        [SerializeField] private SpriteRenderer _defaultSprite, _leftRaceSprite, _rightRaceSprite, _leftRaceToArriveSprite, _rightRaceToArriveSprite;
        [SerializeField] private GameObject _rightRaceMask, _rightRaceToArrive;

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

        public override int Capacity => 2;
        public override bool IsFull => _races.Count >= Capacity;
        public override bool IsEmpty => _races.Count == 0;

        public override void Initialize(Vector3 velocity)
        {
            base.Initialize(velocity);
            _racesToArrive = new List<Race>();
            _racesToDeparture = new List<Race>();
            //TODO: veil
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

        public override void AddRace(Race race, bool hard = false)
        {
            _races.Add(race);
            race.PlanetSaveIndex = SaveIndex;
            RefreshView();
        }

        public override void DepartureRace(Race race)
        {
            _races.Remove(race);
            RefreshView();
        }

        public override void RemoveRaceToArrive(Race race)
        {
            _racesToArrive.Remove(race);
        }

        public override void RemoveRaceToDeparture(Race race)
        {
            _racesToDeparture.Remove(race);
        }

        private void RefreshView()
        {
            var racesCount = _races.Count;
            _leftRaceSprite.gameObject.SetActive(racesCount > 0);
            _leftRaceSprite.sprite = racesCount > 0 ? _races[0].PlanetSprite : null;

            _rightRaceSprite.gameObject.SetActive(racesCount > 1);
            _rightRaceSprite.sprite = racesCount > 1 ? _races[1].PlanetSprite : null;
            _rightRaceMask.SetActive(racesCount > 1);
            //TODO: default sprite
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
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
    }
}