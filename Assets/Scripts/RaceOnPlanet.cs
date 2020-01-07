using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceOnPlanet : PersistableObject
    {
        [SerializeField] private int _population;

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_population);
        }

        public override void Load(GameDataReader reader)
        {
            _population = reader.ReadInt();
        }
    }
}