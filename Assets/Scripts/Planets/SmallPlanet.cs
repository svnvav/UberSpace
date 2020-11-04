using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SmallPlanet : Planet
    {
        [SerializeField] private Race _race;
        [SerializeField] private SpriteRenderer _defaultSprite, _raceSprite, _raceToArriveSprite;
        

        private Race _raceToArrive, _raceToDeparture;
        
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
        
        public override float Radius => 0.5f * _defaultSprite.transform.lossyScale.x;
        
        public override int Capacity => 1;
        public override bool IsFull => _race != null || _raceToArrive != null;
        public override bool IsEmpty => _race == null && _raceToArrive == null || _raceToDeparture != null;

        protected new void Awake()
        {
            base.Awake();
            _veil.AddSpriteRenderer(_defaultSprite);
            _veil.AddSpriteRenderer(_raceSprite);
            _veil.AddSpriteRenderer(_raceToArriveSprite);
        }
        
        private void OnEnable()
        {
            RefreshView();
        }

        public override Race GetRaceByTouchPos(Vector3 touchPos)
        {
            return _race;
        }

        public override void AddRaceToArrive(Race race)
        {
            base.AddRaceToArrive(race);
            
            _raceToArrive = race;
            RefreshView();
        }

        public override void AddRace(Race race, bool hard = false)
        {
            if (race != _raceToArrive && !hard)
            {
                Debug.LogError("RaceToArrive and actual race are different");
            }

            _raceToArrive = null;
            _race = race;
            race.PlanetSaveIndex = SaveIndex;
            RefreshView();
        }
        
        public override void AddRaceToDeparture(Race race)
        {
            base.AddRaceToDeparture(race);
            if (race != _race)
            {
                Debug.LogError("RaceToDeparture and actual race are different");
            }

            _raceToDeparture = race;
        }

        //when taxi takes passenger
        public override void DepartureRace(Race race)
        {
            if (race != _race)
            {
                Debug.LogError("There is no that race to departure");
            }
            if (race != _raceToDeparture)
            {
                Debug.LogError("RaceToDeparture and actual race are different");
            }
            
            _race = null;
            _raceToDeparture = null;
            
            RefreshView();
        }

        public override void RemoveRaceToArrive(Race race)
        {
            if (_raceToArrive != race)
            {
                Debug.LogError("RaceToArrive and race in parameter are different");
            }

            _raceToArrive = null;
            RefreshView();
        }

        public override void RemoveRaceToDeparture(Race race)
        {
            if (_raceToDeparture != race)
            {
                Debug.LogError("RaceToDeparture and race in parameter are different");
            }

            _raceToDeparture = null;
        }

        private void RefreshView()
        {
            _raceSprite.sprite = _race != null ? _race.PlanetSprite : null;
            _raceToArriveSprite.sprite = _raceToArrive != null ? _raceToArrive.PlanetSprite : null;

            _raceSprite.gameObject.SetActive(_race != null);
            _defaultSprite.gameObject.SetActive(_race == null);
            _raceToArriveSprite.gameObject.SetActive(_raceToArrive != null);
            
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
            if (_race != null)
            {
                _race.Die();
            }
            base.Die();
        }

        public override void Recycle()
        {
            _race = null;
            _raceToArrive = null;
            _raceToDeparture = null;
            base.Recycle();
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            RefreshView();
        }
        
    }
}