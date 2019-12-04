
using System;
using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class GameController : PersistableObject
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private PlanetFactory _planetFactory;
        [SerializeField] private PersistentStorage _storage;
        
        private List<Planet> _planets;

        private void Awake()
        {
            _planets = new List<Planet>();
            Instance = this;
        }

        private void Update()
        {

        }

        public void AddPlanet(Planet planet)
        {
            
        }
        
        public override void Save(GameDataWriter writer)
        {
            
        }

        public override void Load(GameDataReader reader)
        {
            
        }
    }
}
