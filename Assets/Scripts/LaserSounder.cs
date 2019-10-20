using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(RainbowShooter))]
    [RequireComponent(typeof(AudioSource))]
    public class LaserSounder : MonoBehaviour
    {
        public AudioClip laserStartClip;
        public AudioClip laserMidClip;
        public AudioClip laserEndClip;

        public float HitVolume = 0.5f;
        public float NothitVolume = 0.1f;
        
        private AudioSource _audioSource;
        private RainbowShooter _laser;

        private Coroutine _laserMidCoroutine;
        
        private void Awake()
        {
            _laser = GetComponent<RainbowShooter>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _laser.OnRainbowStartedShooting += OnRainbowStartedShooting;
            _laser.OnRainbowStoppedShooting += OnRainbowStoppedShooting;
        }

        private void Update()
        {
            _audioSource.volume = _laser.HistLastFrame ? HitVolume : NothitVolume;
        }

        private void OnRainbowStoppedShooting()
        {
            if (_laserMidCoroutine != null)
                StopCoroutine(_laserMidCoroutine);

            _audioSource.clip = laserEndClip;
            _audioSource.Play();
            _audioSource.loop = false;
        }

        private void OnRainbowStartedShooting()
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();

            _audioSource.clip = laserStartClip;
            _audioSource.Play();
            _audioSource.loop = false;
            
            if (_laserMidCoroutine != null)
                StopCoroutine(_laserMidCoroutine);

            _laserMidCoroutine = StartCoroutine(LaserMidCoroutine(laserStartClip.length));
        }

        private IEnumerator LaserMidCoroutine(float initialDelay)
        {
            if (initialDelay > 0)
                yield return new WaitForSeconds(initialDelay);

            _audioSource.clip = laserMidClip;
            _audioSource.Play();
            _audioSource.loop = true;

            _laserMidCoroutine = null;
        }
    }
}