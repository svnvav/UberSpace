﻿using Catlike.ObjectManagement;
using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class CheckpointStage : ScenarioStageBase
    {
        [SerializeField] private float _duration;
        public override float Duration => _duration;

        public override void Begin()
        {
            var levelId = CoreSceneController.Instance.GameStateController.CurrentLevelIndex;
            var stageId = GameLevel.Current.CurrentScenarioStageIndex;
            var fileName = $"{GameController.SaveFileName}_{levelId}_{stageId}";
            PersistentStorage.Instance.Save(GameController.Instance, GameController.SaveVersion, fileName);
            PlayerPrefs.SetInt(fileName, GameLevel.Current.CurrentStarsCount);
            PlayerPrefs.Save();
            CoreSceneController.Instance.CoreDataProvider.UpdateStageData(levelId, stageId);
        }

        public override void Progress(float deltaTime)
        {
        }

        public override void SetTime(float progress)
        {
        }
    }
}