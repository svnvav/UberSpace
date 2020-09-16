using UnityEngine;

namespace Svnvav.UberSpace
{
    public class SpawningStage : ScenarioStageBase
    {
        [SerializeField] private float _duration;
        [SerializeField] private ScenarioStageItem[] _items;
        
        public override float Duration => _duration;

        public override void Progress(float deltaTime)
        {
            foreach (var item in _items)
            {
                item.Progress(deltaTime);
            }
        }

        public override void SetTime(float progress)
        {
            foreach (var item in _items)
            {
                item.SetTime(progress);
            }
        }
    }
}