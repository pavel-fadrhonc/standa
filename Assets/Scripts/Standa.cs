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

        private GameStateManager _gameStateManager;
        private Animator _animator;
        
        private void Awake()
        {
            _rainbowShooter = GetComponent<RainbowShooter>();
            _rainbowShooter.enabled = false;
            _gameStateManager = FindObjectOfType<GameStateManager>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _rainbowShooter.OnRainbowStartedShooting += OnOnRainbowStartedShooting;
            _rainbowShooter.OnRainbowStoppedShooting += OnOnRainbowStoppedShooting;
            
            _gameStateManager.GameStateChanged += OnGameStateChanged;

            EnableShootingVisual(false);
        }

        private void OnGameStateChanged(GameStateManager.eGameState gameState)
        {
            _rainbowShooter.enabled = false;

            if (gameState == GameStateManager.eGameState.Game)
            {
                _rainbowShooter.enabled = true;
            }

            if (gameState == GameStateManager.eGameState.Win)
            {
                EnableShootingVisual(false);
            }
        }

        private void OnOnRainbowStartedShooting()
        {
            EnableShootingVisual(true);
        }        
        
        private void OnOnRainbowStoppedShooting()
        {
            EnableShootingVisual(false);
        }

        private bool _shootingVisualEnabled = false;
        private void LateUpdate()
        {
//            if (_gameStateManager.GameState == GameStateManager.eGameState.Game)
//            {
//                standaShootingSprite.gameObject.SetActive(_shootingVisualEnabled);
//                standaNotShootingSprite.gameObject.SetActive(!_shootingVisualEnabled);
//                handSprite.gameObject.SetActive(!_shootingVisualEnabled);
//            }
        }

        private void EnableShootingVisual(bool enable)
        {
            _shootingVisualEnabled = enable;
            _animator.SetBool("Shooting", enable);
                
            //enabledEffects.ForEach(e => e.gameObject.SetActive(enable));
        }
    }
}