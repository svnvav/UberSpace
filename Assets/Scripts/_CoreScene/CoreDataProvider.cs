using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreDataProvider : MonoBehaviour, IPersistable
    {
        private const string _saveFileName = "CommonSave";
        private const int _saveVersion = 0;
        
        private int _lastLoadedLevel = -1;
        private int _lastLoadedLevelStage = -1;

        public int LastLoadedLevel => _lastLoadedLevel;
        public int LastLoadedLevelStage => _lastLoadedLevelStage;

        private void Awake()
        {
            PersistentStorage.Instance.Load(this, _saveFileName);
        }

        public void UpdateData(int levelId, int stageId)
        {
            _lastLoadedLevel = levelId;
            _lastLoadedLevelStage = stageId;
            PersistentStorage.Instance.Save(this, _saveVersion, _saveFileName);
        }

        public void Save(GameDataWriter writer)
        {
            writer.Write(_lastLoadedLevel);
            writer.Write(_lastLoadedLevelStage);
        }

        public void Load(GameDataReader reader)
        {
            var version = reader.Version;
            if (version > _saveVersion)
            {
                Debug.LogError("Unsupported future save version " + version);
                return;
            }
            
            _lastLoadedLevel = reader.ReadInt();
            _lastLoadedLevelStage = reader.ReadInt();
        }
    }
}