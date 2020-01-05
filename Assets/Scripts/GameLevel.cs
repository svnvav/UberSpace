using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameLevel : PersistableObject
    {
        public static GameLevel Current;
        
        [SerializeField] private GameLevelObject[] _levelObjects;

        private void OnEnable()
        {
            Current = this;
            if (_levelObjects == null)
            {
                _levelObjects = new GameLevelObject[0];
            }
        }

        public void GameUpdate()
        {
            foreach (var levelObject in _levelObjects)
            {
                levelObject.GameUpdate();
            }
        }
        
        public override void Save(GameDataWriter writer)
        {
            writer.Write(_levelObjects.Length);
            foreach (var _persistentObject in _levelObjects)
            {
                _persistentObject.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            var savedCount = reader.ReadInt();
            for (int i = 0; i < _levelObjects.Length; i++)
            {
                _levelObjects[i].Load(reader);
            }
        }
    }
}