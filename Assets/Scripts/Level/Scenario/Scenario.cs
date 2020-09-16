using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Scenario : LevelObject
    {
        private static float AdditionalDeltaTimeForLastStageUpdate = 0.001f;//to avoid precision error
        
        [SerializeField] private ScenarioStageBase[] _stages;

        private int _currentStageId;
        
        private float _progress;

        public override void GameUpdate(float deltaTime)
        {
            var dt = deltaTime;
            _progress += dt;
            
            while (_currentStageId < _stages.Length && _progress > _stages[_currentStageId].Duration)
            {
                var lastDt = _stages[_currentStageId].Duration - (_progress - dt) + AdditionalDeltaTimeForLastStageUpdate;
                _stages[_currentStageId].Progress(lastDt);
                dt -= lastDt;
                _progress -= _stages[_currentStageId].Duration;
                _currentStageId++;
            }
            
            if(_currentStageId >= _stages.Length) return;
            
            _stages[_currentStageId].Progress(dt);
        }

        public override void Save (GameDataWriter writer) {
            writer.Write(_currentStageId);
            writer.Write(_progress);
        }

        public override void Load (GameDataReader reader) {
            _currentStageId = reader.ReadInt();
            _progress = reader.ReadFloat();
            if (_currentStageId < _stages.Length)
            {
                _stages[_currentStageId].SetTime(_progress);
            }
        }
    }
}