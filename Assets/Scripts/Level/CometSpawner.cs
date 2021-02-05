using UnityEngine;

namespace Svnvav.UberSpace
{
    public class CometSpawner : Spawner
    {
        [SerializeField] private GameObject _comet;
        
        public override void Spawn(float speed)
        {
            _comet.SetActive(true);
            //TODO:
        }
    }
}