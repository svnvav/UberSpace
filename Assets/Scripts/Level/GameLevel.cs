using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameLevel : MonoBehaviour, IPersistable
    {
        public static GameLevel Current;
        
        [SerializeField] private LevelObject[] _levelObjects;

        private void OnEnable()
        {
            Current = this;
            if (_levelObjects == null)
            {
                _levelObjects = new LevelObject[0];
            }
        }

        public void GameUpdate(float deltaTime)
        {
            foreach (var levelObject in _levelObjects)
            {
                if(levelObject.isActiveAndEnabled) levelObject.GameUpdate(deltaTime);
            }
        }
        
        public void Save(GameDataWriter writer)
        {
            writer.Write(_levelObjects.Length);
            foreach (var levelObject in _levelObjects)
            {
                levelObject.Save(writer);
            }
        }

        public void Load(GameDataReader reader)
        {
            var savedCount = reader.ReadInt();//TODO: is it needed?
            for (int i = 0; i < _levelObjects.Length; i++)
            {
                _levelObjects[i].Load(reader);
            }
        }
    }
}