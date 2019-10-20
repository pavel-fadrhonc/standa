using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LaserShootCameraShake : MonoBehaviour
    {
        public RainbowShooter rainbowShooter;
        public ShakeBehaviour shakeBehaviour;

        public float startScreenShakeMag = 0.05f;
        public float endScreenShakeMag = 0.15f;

        public float maxScreenShakeTime = 4.0f;

        public float _shootingTime;
        
        private void Awake()
        {
            rainbowShooter.OnRainbowStartedShooting += OnRainbowStartedShooting;
            rainbowShooter.OnRainbowStoppedShooting += OnRainbowStoppedShooting;
        }

        private void Update()
        {
            _shootingTime += Time.deltaTime;
            shakeBehaviour.shakeMagnitude = Mathf.Lerp(startScreenShakeMag, endScreenShakeMag, _shootingTime / maxScreenShakeTime);
        }

        private void OnRainbowStoppedShooting()
        {
            shakeBehaviour.enabled = false;
        }

        private void OnRainbowStartedShooting()
        {
            shakeBehaviour.enabled = true;
            _shootingTime = 0;
        }
    }
}