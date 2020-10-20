using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameLevel : MonoBehaviour, IPersistable
    {
        public static GameLevel Current;
        
        [SerializeField] private Scenario _scenario;
        [SerializeField] private LevelObject[] _levelObjects;
        [SerializeField] private int _maxStarsCount;

        private int _currentStarsCount;
        
        public int CurrentScenarioStageIndex => _scenario.CurrentStageId;

        public int MaxStarsCount => _maxStarsCount;

        public int CurrentStarsCount => _currentStarsCount;

        private void OnEnable()
        {
            Current = this;
            _currentStarsCount = _maxStarsCount;
            if (_levelObjects == null)
            {
                _levelObjects = new LevelObject[0];
            }
        }

        public void DecreaseUberStar()
        {
            if(_currentStarsCount <= 0) return;
            _currentStarsCount--;
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
            writer.Write(_currentStarsCount);
            _scenario.Save(writer);
            
            writer.Write(_levelObjects.Length);
            foreach (var levelObject in _levelObjects)
            {
                levelObject.Save(writer);
            }
        }

        public void Load(GameDataReader reader)
        {
            _currentStarsCount = reader.ReadInt();
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