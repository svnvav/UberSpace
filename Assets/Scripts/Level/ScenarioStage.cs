using System;

namespace Svnvav.UberSpace
{
    [Serializable]
    public class ScenarioStage
    {
        public float _duration;
        public ScenarioStageItem[] Items;

        public void Progress(float deltaTime)
        {
            foreach (var item in Items)
            {
                item.Progress(deltaTime);
            }
        }

        public void SetTime(float progress)
        {
            foreach (var item in Items)
            {
                item.SetTime(progress);
            }
        }
    }
}