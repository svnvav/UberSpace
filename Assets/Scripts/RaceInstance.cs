using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceInstance : PersistableObject
    {
        [SerializeField] private RaceInfo _info;

        public RaceInfo Info {
            get
            {
                if (_info == null)
                {
                    _info = OriginFactory.Infos[_infoId];//TODO: bad code
                }
                return _info;
            }
        }

        [SerializeField] private int _population;

        #region Factory

        private int _infoId = int.MinValue;

        private RaceFactory _originFactory;

        public int InfoId
        {
            get => _infoId;
            set
            {
                if (_infoId == int.MinValue)
                {
                    _infoId = value;
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
            writer.Write(InfoId);
            writer.Write(_population);
        }

        public override void Load(GameDataReader reader)
        {
            InfoId = reader.ReadInt();
            _population = reader.ReadInt();
        }
    }
}