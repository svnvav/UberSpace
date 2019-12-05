using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Planet : PersistableObject
    {
        [SerializeField] private List<RaceOnPlanet> _races;

        #region Factory
        
        private int _idInFactory = int.MinValue;

        private PlanetFactory _originFactory;

        public int IdInFactory {
            get => _idInFactory;
            set {
                if (_idInFactory == int.MinValue) {
                    _idInFactory = value;
                }
                else {
                    Debug.LogError("Not allowed to change IdInFactory.");
                }
            }
        }
        
        public PlanetFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                if (_originFactory == null) {
                    _originFactory = value;
                }
                else {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }
        
        #endregion
        
        public void Recycle ()
        {
            OriginFactory.Reclaim(this);
        }


        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
        }
    }
}