using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BarUpdater : MonoBehaviour
    {
        public HaveStats haveStats;
        public eStatType stat;

        public Image barImage;
        public Image barMissedImage;

        public float missedHealthShowTime = 2.5f;

        private FloatStat _stat;

        private Coroutine _refreshMissedHealthImageCor;

        private void Start()
        {
            _stat = haveStats.GetFloatStat(stat);
            _stat.OnValueChanged += OnStatChanged;
        }

        private void OnStatChanged(FloatStat stat, float oldValue, float newValue)
        {
            if (!enabled || !gameObject.activeSelf)
                return;
            
            barImage.fillAmount = newValue / stat.MaxValue;

            if (newValue > oldValue)
                return;
            
            if (_refreshMissedHealthImageCor != null)
                StopCoroutine(_refreshMissedHealthImageCor);

            _refreshMissedHealthImageCor = StartCoroutine(RefreshMissedHealthImage());
        }

        private IEnumerator RefreshMissedHealthImage()
        {
            yield return new WaitForSeconds(missedHealthShowTime);
            
            barMissedImage.fillAmount = _stat.Value / _stat.MaxValue;

            _refreshMissedHealthImageCor = null;
        }
    }
}