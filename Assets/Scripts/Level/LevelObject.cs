using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public abstract class LevelObject : MonoBehaviour, IGameUpdatable, IPersistable
    {
        public abstract void GameUpdate(float deltaTIme);

        public abstract void Save(GameDataWriter writer);

        public abstract void Load(GameDataReader reader);
    }
}