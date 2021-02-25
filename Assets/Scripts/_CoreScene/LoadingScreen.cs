using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace.CoreScene
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;

        public void SetProgress(float value)
        {
            _progressBar.value = value;
        }
    }
}