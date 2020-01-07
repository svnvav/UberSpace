using UnityEngine;

namespace Catlike.ObjectManagement
{
    [DisallowMultipleComponent]
    public class PersistableObject : MonoBehaviour
    {
        public virtual void Save (GameDataWriter writer) {}

        public virtual void Load (GameDataReader reader) {}
    }
}