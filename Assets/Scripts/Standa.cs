using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Standa : MonoBehaviour
    {
        public SpriteRenderer standaShootingSprite;
        public SpriteRenderer standaNotShootingSprite;
        public SpriteRenderer handSprite;

        public List<GameObject> enabledEffects = new List<GameObject>();
        
        private RainbowShooter _rainbowShooter;
        
        private void Awake()
        {
            _rainbowShooter = GetComponent<RainbowShooter>();
        }

        private void Start()
        {
            _rainbowShooter.OnRainbowStartedShooting += OnOnRainbowStartedShooting;
            _rainbowShooter.OnRainbowStoppedShooting += OnOnRainbowStoppedShooting;

            EnableShootingVisual(false);
        }

        private void OnOnRainbowStartedShooting()
        {
            EnableShootingVisual(true);
        }        
        
        private void OnOnRainbowStoppedShooting()
        {
            EnableShootingVisual(false);
        }

        private void EnableShootingVisual(bool enable)
        {
            standaShootingSprite.gameObject.SetActive(enable);
            standaNotShootingSprite.gameObject.SetActive(!enable);
            handSprite.gameObject.SetActive(!enable);
            
            enabledEffects.ForEach(e => e.gameObject.SetActive(enable));
        }
    }
}