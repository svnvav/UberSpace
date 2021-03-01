using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Svnvav.UberSpace.CoreScene
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _background;
        [SerializeField] private float _animDuration;

        private Image _backgroundImage;
        private Image _sliderBackgroundImage;
        private Image _sliderFillImage;

        private float _sliderBackgroundImageAlpha;

        private void Awake()
        {
            _backgroundImage = _background.GetComponent<Image>();
            _sliderBackgroundImage = _slider.transform.GetChild(0).GetComponent<Image>();
            _sliderFillImage = _slider.fillRect.GetComponent<Image>();
            _sliderBackgroundImageAlpha = _sliderBackgroundImage.color.a;
        }

        public IEnumerator ShowRoutine()
        {
            _background.SetActive(true);
            _slider.gameObject.SetActive(true);
            var t = 0f;
            while (t <= 1f)
            {
                t += Time.deltaTime / _animDuration;
                SetAlpha(t);
                yield return null;
            }
        }
        public IEnumerator HideRoutine()
        {
            var t = 1f;
            while (t >= 0f)
            {
                t -= Time.deltaTime / _animDuration;
                SetAlpha(t);
                yield return null;
            }
            _background.SetActive(false);
            _slider.gameObject.SetActive(false);
        }

        private void SetAlpha(float value)
        {
            var col = _backgroundImage.color;
            col.a = Mathf.Lerp(0, 1, value);
            _backgroundImage.color = col; 
            
            col = _sliderFillImage.color;
            col.a = Mathf.Lerp(0, 1, value);
            _sliderFillImage.color = col;
            
            col = _sliderBackgroundImage.color;
            col.a = Mathf.Lerp(0, _sliderBackgroundImageAlpha, value);
            _sliderBackgroundImage.color = col;
        }
        
        public void SetProgress(float value)
        {
            _slider.value = value;
        }
    }
}