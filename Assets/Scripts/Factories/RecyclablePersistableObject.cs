namespace Svnvav.UberSpace
{
    public class RecyclablePersistableObject
    {
        void SetOriginFactory<T>(PrefabGenericFactory<T> originGenericFactory) where T : Object, IRecyclable;

        int PrefabId { get; set; }
        
        GameObject RecyclableGameObject { get; }
    }
}