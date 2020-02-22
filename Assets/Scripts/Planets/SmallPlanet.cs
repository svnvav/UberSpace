using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SmallPlanet : Planet
    {
        [SerializeField] private Race _race;
        [SerializeField] private SpriteRenderer _defaultSprite, _raceSprite;
        [SerializeField] private Color _veilColor;

        private int _saveIndex;
        public override int SaveIndex
        {
            get => _saveIndex;
            set
            {
                _saveIndex = value;
                if (_race != null)
                {
                    _race.PlanetSaveIndex = _saveIndex;
                }
            }
        }
        
        public override int Capacity => 1;
        public override bool IsFull => _race != null;
        public override bool IsEmpty => _race == null;
        public override void Veil()
        {
            _defaultSprite.color = _veilColor;
            _raceSprite.color = _veilColor;
        }

        public override void Unveil()
        {
            _defaultSprite.color = Color.white;
            _raceSprite.color =  Color.white;
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            return _race;
        }

        public override void AddRace(Race race)
        {
            base.AddRace(race);

            _race = race;
            RefreshView();
        }

        public override void RemoveRace(Race race)
        {
            if (race != _race)
            {
                throw new Exception("There is no that race");
            }
            _race = null;
            RefreshView();
        }
        
        private void RefreshView()
        {
            _raceSprite.sprite = _race != null ? _race.PlanetSprite : null;

            _raceSprite.gameObject.SetActive(IsFull);
            _defaultSprite.gameObject.SetActive(IsEmpty);
        }

        public override void Die()
        {
            if (_race != null)
            {
                _race.Die();
            }
            base.Die();
        }

        public override void Recycle()
        {
            base.Recycle();
            _race = null;
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
        
    }
}