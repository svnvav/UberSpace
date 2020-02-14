using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameLevel : PersistableObject
    {
        public static GameLevel Current;

        [SerializeField] private PlanetFactory _planetFactory;
        [SerializeField] private RaceFactory _raceFactory;
        [SerializeField] private GameLevelObject[] _levelObjects;

        public PlanetFactory PlanetFactory => _planetFactory;
        public RaceFactory RaceFactory => _raceFactory;

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
                if(levelObject.isActiveAndEnabled) levelObject.GameUpdate();
            }
        }
        
        public override void Save(GameDataWriter writer)
        {
            writer.Write(_levelObjects.Length);
            foreach (var levelObject in _levelObjects)
            {
                levelObject.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            var savedCount = reader.ReadInt();//TODO: is it needed?
            for (int i = 0; i < _levelObjects.Length; i++)
            {
                _levelObjects[i].Load(reader);
            }
        }
    }
}