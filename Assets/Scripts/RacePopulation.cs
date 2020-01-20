using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RacePopulation : PersistableObject
    {
        [SerializeField] private int _infoId;

        public RaceInfo Info => GameController.Instance.AllRacesInfo.Infos[_infoId];

        [SerializeField] private int _population;

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_infoId);
            writer.Write(_population);
        }

        public override void Load(GameDataReader reader)
        {
            _infoId = reader.ReadInt();
            _population = reader.ReadInt();
        }
    }
}