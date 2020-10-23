﻿using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace.CoreScene
{
    public class CoreSceneController : MonoBehaviour
    {
        public static CoreSceneController Instance;
        
        [SerializeField] private CoreDataProvider _coreDataProvider;
        [SerializeField] private GameStateController _gameStateController;

        public CoreDataProvider CoreDataProvider => _coreDataProvider;
        public GameStateController GameStateController => _gameStateController;

        private void Awake()
        {
            Instance = this;
        }

        public void ContinueGame()
        {
            var saveFileName = 
                $"{GameController.SaveFileName}_{CoreDataProvider.LastLoadedLevelPostfix}_{CoreDataProvider.LastLoadedLevelStage}";
            LoadLevel(saveFileName);
            //TODO: fix planets veil bug after loading
        }

        public void LoadLastCheckpoint()
        {
            _gameStateController.LoadLast();
        }

        public void LoadLevel(string saveFileName)
        {
            _gameStateController.GoToLevel(saveFileName);
        }

        public void GoToMainMenu()
        {
            _gameStateController.GoToMainMenu();
        }
    }
}