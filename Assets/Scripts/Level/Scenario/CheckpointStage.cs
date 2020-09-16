using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class CheckpointStage : ScenarioStageBase
    {
        [SerializeField] private float _duration;
        public override float Duration => _duration;

        private bool _saved;
        
        public override void Progress(float deltaTime)
        {
            if (!_saved)
            {
                //TODO: GameController.Instance
            }
        }

        public override void SetTime(float progress)
        {
            
        }
    }
}