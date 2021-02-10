using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreDataProvider : MonoBehaviour, IPersistable
    {
        private const string SAVE_FILE_NAME = "CommonSave";
        private const int SAVE_VERSION = 0;
        
        [SerializeField] private int _cometsCount = 20;
        
        private int _lastLoadedLevel = -1;
        private int _lastLoadedLevelStage = -1;
        private bool[] _cometCatchFlags;

        public int LastLoadedLevel => _lastLoadedLevel;
        public int LastLoadedLevelStage => _lastLoadedLevelStage;

        private void Awake()
        {
            _cometCatchFlags = new bool[_cometsCount];
            PersistentStorage.Instance.Load(this, SAVE_FILE_NAME);
        }

        private void OnApplicationQuit()
        {
            SaveGlobalData();
        }

        public void OnCometCaught(int cometId)
        {
            _cometCatchFlags[cometId] = true;
        }

        public bool IsCometCaught(int cometId)
        {
            return _cometCatchFlags[cometId];
        }
        
        public void UpdateStageData(int levelId, int stageId)
        {
            _lastLoadedLevel = levelId;
            _lastLoadedLevelStage = stageId;
        }

        public void SaveGlobalData()
        {
            PersistentStorage.Instance.Save(this, SAVE_VERSION, SAVE_FILE_NAME);
        }

        public void Save(GameDataWriter writer)
        {
            writer.Write(_lastLoadedLevel);
            writer.Write(_lastLoadedLevelStage);
            writer.Write(_cometsCount);
            for (int i = 0; i < _cometsCount; i++)
            {
                writer.Write(_cometCatchFlags[i]);
            }
        }

        public void Load(GameDataReader reader)
        {
            var version = reader.Version;
            if (version > SAVE_VERSION)
            {
                Debug.LogError("Unsupported future save version " + version);
                return;
            }
            
            _lastLoadedLevel = reader.ReadInt();
            _lastLoadedLevelStage = reader.ReadInt();
            var oldCometsCount = reader.ReadInt();
            for (int i = 0; i < oldCometsCount; i++)
            {
                _cometCatchFlags[i] = reader.ReadBool();
            }
        }
    }
}