using UnityEngine;

namespace Svnvav.UberSpace
{
    public class CometSpawner : Spawner
    {
        [SerializeField] private int _cometId;
        [SerializeField] private Comet _comet;

        public override void Spawn(float speed)
        {
            _comet.Initialize(_cometId, Vector3.Normalize(transform.up) * speed);
            _comet.gameObject.SetActive(true);
        }
    }
}