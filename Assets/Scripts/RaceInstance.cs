using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceInstance : PersistableObject
    {
        [SerializeField] private RaceInfo _info;

        public RaceInfo Info => _info;

        [SerializeField] private int _population;

        #region Factory

        private int _idInFactory = int.MinValue;

        private RaceFactory _originFactory;

        public int IdInFactory
        {
            get => _idInFactory;
            set
            {
                if (_idInFactory == int.MinValue)
                {
                    _idInFactory = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }

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

        
        public override void Save(GameDataWriter writer)
        {
            writer.Write(IdInFactory);
            writer.Write(_population);
        }

        public override void Load(GameDataReader reader)
        {
            IdInFactory = reader.ReadInt();
            _population = reader.ReadInt();
        }
    }
}