using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreDataProvider : MonoBehaviour, IPersistable
    {
        private const string _saveFileName = "CommonSave";
        private const int _saveVersion = 0;
        
        private string _lastLoadedLevelPostfix;

        private void Awake()
        {
            PersistentStorage.Instance.Load(this, _saveFileName);
        }

        public void Save(GameDataWriter writer)
        {
            writer.Write(_lastLoadedLevelPostfix);
        }

        public void Load(GameDataReader reader)
        {
            var version = reader.Version;
            if (version > _saveVersion)
            {
                Debug.LogError("Unsupported future save version " + version);
                return;
            }
            
            _lastLoadedLevelPostfix = reader.ReadString();
        }

        public void UpdateSavedData()
        {
            PersistentStorage.Instance.Save(this, _saveVersion, _saveFileName);
        }
    }
}