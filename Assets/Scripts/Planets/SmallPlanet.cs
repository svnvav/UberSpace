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

        public override bool AddRace(Race race)
        {
            if (IsFull)
            {
                Debug.LogError("Trying to put a race to full planet");
                return false;
            }

            _race = race;
            RefreshView();
            return true;
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
            _raceSprite.gameObject.SetActive(IsFull);
            _defaultSprite.gameObject.SetActive(IsEmpty);
        }
        
        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
        
    }
}