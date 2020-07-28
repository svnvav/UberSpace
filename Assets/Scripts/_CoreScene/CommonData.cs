using Catlike.ObjectManagement;

namespace Svnvav.UberSpace.CoreScene
{
    public class CommonData
    {
        private string _lastLoadedLevelPostfix;
        
        public void Save(GameDataWriter writer)
        {
            writer.Write(_lastLoadedLevelPostfix);
        }

        public void Load(GameDataReader reader)
        {
            _lastLoadedLevelPostfix = reader.ReadString();
        }
    }
}