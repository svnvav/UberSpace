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

        private int _currentItemId;
        
        private float _progress;

        public override void GameUpdate()
        {
            
        }

        public override void Save (GameDataWriter writer) {
            writer.Write(_progress);
        }

        public override void Load (GameDataReader reader) {
            _progress = reader.ReadFloat();
        }
    }
}