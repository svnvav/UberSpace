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

        public override bool IsFull => _races.Count >= 2;
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

        public override bool AddRace(Race race)
        {
            if (_races.Count >= 2)
            {
                Debug.LogError("Trying to put a race to full planet");
                return false;
            }
            
            _races.Add(race);
            race.PlanetSaveIndex = SaveIndex;
            RefreshView();
            return true;
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
            base.Die();
            foreach (var race in _races)
            {
                race.Die();
            }
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