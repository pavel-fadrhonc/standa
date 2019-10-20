using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameStateManager : MonoBehaviour
    {
        public enum eGameState
        {
            StartMenu,
            IntroAnim,
            Game,
            Death,
            Win,
        }

        [Header("Start Game")] 
        public Button startGameButton;
        
        [Header("Game")]
        public GameObject Standa;
        public AudioSource backGroundMusic;
        public List<GameObject> gameEnableObjects = new List<GameObject>();
        
        private FloatStat _standaHealthStat;

        private void Awake()
        {
            startGameButton.onClick.AddListener((() => ChangeGameState(eGameState.Game)));
            
        }

        public void ChangeGameState(eGameState gameState)
        {
            startGameButton.gameObject.SetActive(false);
            Standa.gameObject.SetActive(false);
            gameEnableObjects.ForEach(g => g.SetActive(false));
            
            switch (gameState)
            {
                case eGameState.StartMenu:
                    startGameButton.gameObject.SetActive(true);
                    break;
                case eGameState.Game:
                    backGroundMusic.Play();
                    Standa.gameObject.SetActive(true);
                    gameEnableObjects.ForEach(g => g.SetActive(true));
                    break;
                case eGameState.Death:
                    break;
                case eGameState.Win:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }
        
        private void Start()
        {
            _standaHealthStat = Standa.GetComponent<HaveStats>().GetFloatStat(eStatType.Health);
            _standaHealthStat.OnValueChanged += OnStandaHealthChanged;
            
            ChangeGameState(eGameState.StartMenu);
        }

        private void OnStandaHealthChanged(FloatStat healthStat, float oldVal, float newVal)
        {
            if (newVal <= 0)
            { // player Death
                
            }
        }
    }
}