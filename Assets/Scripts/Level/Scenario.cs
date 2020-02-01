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
        }
        
        [SerializeField] private Stage[] _stages;

        private int _currentStageId;
        
        private float _progress;

        public override void GameUpdate()
        {
            _progress += Time.deltaTime;
            
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