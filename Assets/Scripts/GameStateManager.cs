using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
        
        [Header("Intro Anim")]
        public List<GameObject> introAnimEnabledObjects = new List<GameObject>();
        public PlayableDirector introSequenceDirector;

        [Header("Game")] 
        public GameObject standa;
        public AudioSource backGroundMusic;
        public List<GameObject> gameEnableObjects = new List<GameObject>();

        private Animator _standaAnimator;        
        private FloatStat _standaHealthStat;

        private void Awake()
        {
            startGameButton.onClick.AddListener((() => ChangeGameState(eGameState.IntroAnim)));
            introSequenceDirector.stopped += director => ChangeGameState(eGameState.Game);
            introSequenceDirector.paused += director => ChangeGameState(eGameState.Game);
            _standaAnimator = standa.GetComponentInChildren<Animator>();
        }

        public void ChangeGameState(eGameState gameState)
        {
            startGameButton.gameObject.SetActive(false);
            //Standa.gameObject.SetActive(false);
            gameEnableObjects.ForEach(g => g.SetActive(false));
            introAnimEnabledObjects.ForEach(g => g.SetActive(false));
            
            switch (gameState)
            {
                case eGameState.StartMenu:
                    startGameButton.gameObject.SetActive(true);
                    _standaAnimator.SetTrigger("Intro");
                    break;
                case eGameState.IntroAnim:
                    introAnimEnabledObjects.ForEach(g => g.SetActive(true));
                    break;
                case eGameState.Game:
                    backGroundMusic.Play();
                    gameEnableObjects.ForEach(g => g.SetActive(true));
                    _standaAnimator.SetTrigger("Riding");
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
            _standaHealthStat = standa.GetComponent<HaveStats>().GetFloatStat(eStatType.Health);
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