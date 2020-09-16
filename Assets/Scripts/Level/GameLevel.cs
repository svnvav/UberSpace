using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameLevel : MonoBehaviour, IPersistable
    {
        public static GameLevel Current;

        [SerializeField] private Scenario _scenario;
        [SerializeField] private LevelObject[] _levelObjects;

        public int CurrentScenarioStageIndex => _scenario.CurrentStageId;

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
            _scenario.GameUpdate(deltaTime);
            
            foreach (var levelObject in _levelObjects)
            {
                if(levelObject.isActiveAndEnabled) levelObject.GameUpdate(deltaTime);
            }
        }
        
        public void Save(GameDataWriter writer)
        {
            _scenario.Save(writer);
            
            writer.Write(_levelObjects.Length);
            foreach (var levelObject in _levelObjects)
            {
                levelObject.Save(writer);
            }
        }

        public void Load(GameDataReader reader)
        {
            _scenario.Load(reader);
            
            var savedCount = reader.ReadInt();
            if (savedCount != _levelObjects.Length)
            {
                Debug.LogError("LevelObjects.Count is not match with saved number");
            }
            for (int i = 0; i < _levelObjects.Length; i++)
            {
                _levelObjects[i].Load(reader);
            }
        }
    }
}