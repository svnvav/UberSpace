using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreDataProvider : MonoBehaviour, IPersistable
    {
        private const string _saveFileName = "CommonSave";
        private const int _saveVersion = 0;
        
        private int _lastLoadedLevelPostfix;

        public int LastLoadedLevelPostfix
        {
            get => _lastLoadedLevelPostfix;
            set => _lastLoadedLevelPostfix = value;
        }

        private void Awake()
        {
            PersistentStorage.Instance.Load(this, _saveFileName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _lastLoadedLevelPostfix = CoreSceneController.Instance.GameStateController.CurrentLevelIndex;
                PersistentStorage.Instance.Save(this, _saveVersion, _saveFileName);
            }
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
            
            _lastLoadedLevelPostfix = reader.ReadInt();
        }
    }
}