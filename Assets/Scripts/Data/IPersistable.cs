
namespace Catlike.ObjectManagement
{
    public interface IPersistable
    {
        void Save(GameDataWriter writer);

        void Load(GameDataReader reader);
    }
}