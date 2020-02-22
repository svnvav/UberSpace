using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public sealed class BigPlanet : Planet
    {
        [SerializeField] private List<Race> _races;
        [SerializeField] private SpriteRenderer _defaultSprite, _leftRaceSprite, _rightRaceSprite;
        [SerializeField] private GameObject _spriteMask;
        [SerializeField] private Color _veilColor;

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

        private void OnEnable()
        {
            RefreshView();
        }

        public override void Veil()
        {
            _defaultSprite.color = _veilColor;
            _rightRaceSprite.color = _veilColor;
            _leftRaceSprite.color = _veilColor;
        }

        public override void Unveil()
        {
            _defaultSprite.color = Color.white;
            _rightRaceSprite.color =  Color.white;
            _leftRaceSprite.color =  Color.white;
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            var id = _races.Count == 1 ? 
                0 : 
                (touchPos.x - transform.position.x < 0 ? 0 : 1);//0 for left and 1 for right
            
            return _races[id];
        }

        public override void AddRace(Race race)
        {
            base.AddRace(race);
            
            _races.Add(race);
            RefreshView();
        }

        public override void RemoveRace(Race race)
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