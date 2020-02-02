using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Scenario : GameLevelObject
    {
        [Serializable]
        private struct Stage
        {
            public Spawner[] Spawner;
            public float _duration;

            public void Progress(float deltaTime)
            {
                foreach (var spawner in Spawner)
                {
                    spawner.Progress(deltaTime);
                }
            }
        }
        
        [SerializeField] private Stage[] _stages;

        private int _currentStageId;
        
        private float _progress;

        public override void GameUpdate()
        {
            var deltaTime = Time.deltaTime;
            _progress += deltaTime;
            
            while (_progress > _stages[_currentStageId]._duration && _currentStageId < _stages.Length)
            {
                var lastDt = _stages[_currentStageId]._duration - (_progress - deltaTime);
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
        }
    }
}