using System.Collections.Generic;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Planet : PersistableObject
    {
        [SerializeField] private List<RaceOnPlanet> _races;
    }
}