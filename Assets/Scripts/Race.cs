using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Race : PersistableObject
    {
        #region StaticInfo

        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private string _tagline;
        public string Tagline => _tagline;

        [SerializeField] private bool _isAggressive;
        public bool IsAggressive => _isAggressive;

        [SerializeField] private Sprite _planetSprite;
        [SerializeField] private Sprite _hudInfoSprite;
        [SerializeField] private Sprite _loadingSprite;

        public Sprite PlanetSprite => _planetSprite;
        public Sprite HudInfoSprite => _hudInfoSprite;
        public Sprite LoadingSprite => _loadingSprite;

        #endregion

        [SerializeField] private int _population;
        public int PlanetSaveIndex { get; set; } = int.MinValue;

        public int SaveIndex { get; set; }

        #region Factory

        private int _prefabId = int.MinValue;

        public int PrefabId
        {
            get => _prefabId;
            set
            {
                if (_prefabId == int.MinValue)
                {
                    _prefabId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }

        private RaceFactory _originFactory;

        public RaceFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                if (_originFactory == null)
                {
                    _originFactory = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }

        #endregion

        public void Die()
        {
            GameController.Instance.RemoveRace(this);
            Recycle();
        }

        public void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(PlanetSaveIndex);
            writer.Write(_population);
        }

        public override void Load(GameDataReader reader)
        {
            PlanetSaveIndex = reader.ReadInt();
            _population = reader.ReadInt();
        }
    }
}