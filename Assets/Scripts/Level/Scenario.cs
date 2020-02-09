using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Scenario : GameLevelObject
    {
        private static float AdditionalDeltaTimeForLastStageUpdate = 0.001f;//to avoid precision error
        
        [SerializeField] private ScenarioStage[] _stages;

        private int _currentStageId;
        
        private float _progress;

        public override void GameUpdate()
        {
            var deltaTime = Time.deltaTime;
            _progress += deltaTime;
            
            while (_currentStageId < _stages.Length && _progress > _stages[_currentStageId]._duration)
            {
                var lastDt = _stages[_currentStageId]._duration - (_progress - deltaTime) + AdditionalDeltaTimeForLastStageUpdate;
                _stages[_currentStageId].Progress(lastDt);
                deltaTime -= lastDt;
                _progress -= _stages[_currentStageId]._duration;
                _currentStageId++;
            }
            
            if(_currentStageId >= _stages.Length) return;
            
            _stages[_currentStageId].Progress(deltaTime);
        }

        public override void Save (GameDataWriter writer) {
            writer.Write(_currentStageId);
            writer.Write(_progress);
        }

        public override void Load (GameDataReader reader) {
            _currentStageId = reader.ReadInt();
            _progress = reader.ReadFloat();
            _stages[_currentStageId].SetTime(_progress);
        }
    }
}