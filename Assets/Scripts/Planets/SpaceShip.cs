using System;
using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpaceShip : Planet
    {
        [Header("Self")]
        [SerializeField] private List<Race> _races;
        [SerializeField] private int _capacity;
        
        [SerializeField] private SpriteRenderer _shipSpriteRenderer;
        [SerializeField] private Sprite _defaultSeatSprite;
        [SerializeField] private SpriteRenderer[] _racesSpriteRenderers;
        [SerializeField] private SpriteRenderer[] _racesToArriveSpriteRenderers;
        
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
        
        public override float Radius => 0.5f * _shipSpriteRenderer.transform.lossyScale.x;
        public override bool IsFull => _races.Count + _racesToArrive.Count >= Capacity;
        public override bool IsEmpty => _races.Count + _racesToArrive.Count == 0 || _racesToDeparture.Count == _races.Count;

        private void Awake()
        {
            _racesToArrive = new List<Race>();
            _racesToDeparture = new List<Race>();
            
            base.Awake();
        }

        public override void GameUpdate(float deltaTime)
        {
            transform.Translate(deltaTime * _velocity.x * transform.up, Space.World);
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            return _races[0];
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
            for (int i = 0; i < _racesToArrive.Count; i++)
            {
                _racesToArriveSpriteRenderers[i].gameObject.SetActive(true);
                _racesToArriveSpriteRenderers[i].sprite = _racesToArrive[i].PlanetSprite;
            }

            for (int i = _racesToArrive.Count; i < _racesToArriveSpriteRenderers.Length; i++)
            {
                _racesToArriveSpriteRenderers[i].gameObject.SetActive(false);
                _racesToArriveSpriteRenderers[i].sprite = _defaultSeatSprite;
            }
            
            for (int i = 0; i < _races.Count; i++)
            {
                _racesSpriteRenderers[i].gameObject.SetActive(true);
                _racesSpriteRenderers[i].sprite = _races[i].PlanetSprite;
            }

            for (int i = _races.Count; i < _racesSpriteRenderers.Length; i++)
            {
                _racesSpriteRenderers[i].gameObject.SetActive(false);
                _racesSpriteRenderers[i].sprite = _defaultSeatSprite;
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
                race.Survive();
            }
            base.Die();
        }
        public override void Recycle()
        {
            _races.Clear();
            _racesToArrive.Clear();
            _racesToDeparture.Clear();
            base.Recycle();
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
    }
}