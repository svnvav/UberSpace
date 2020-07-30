using System.IO;
using UnityEngine;

namespace Catlike.ObjectManagement
{
    public class PersistentStorage : MonoBehaviour
    {
        public static PersistentStorage Instance;
        
        private string _saveFolderPath;

        private void Awake()
        {
            Instance = this;
            _saveFolderPath = Application.persistentDataPath;
        }
        
        public void Save (IPersistable o, int version, string filename) 
        {
            var path = Path.Combine(_saveFolderPath, filename);
            
            using (
                var writer = new BinaryWriter(File.Open(path, FileMode.Create))
            ) {
                writer.Write(-version);
                o.Save(new GameDataWriter(writer));
            }
        }

        public void Load (IPersistable o, string filename)
        {
            var path = Path.Combine(_saveFolderPath, filename);
            var data = File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(data);
            var reader = new BinaryReader(ms);
            o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
    }
}