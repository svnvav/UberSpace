using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Scenario : GameLevelObject
    {
        [SerializeField] private ScenarioItem[] _sequence;

        private int _currentItemId;
        
        private float _progress;
        
        public override void Save (GameDataWriter writer) {
            writer.Write(_progress);
        }

        public override void Load (GameDataReader reader) {
            _progress = reader.ReadFloat();
        }
    }
}