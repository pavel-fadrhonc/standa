using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        
        [Header("Death")]
        public Camera camera;
        public Transform deathTransform;
        public float deathCameraSize;
        public float deathCameraZoomTime = 4f;
        public float deathZoomSpeed = 6;
        public LineRenderer cross;
        public RectTransform crossStart;
        public RectTransform crossEnd;

        [Header("Win")] 
        public Destroyable winAsteroid;
        public float winAsteroidSpeedownTime;
        public AnimationCurve speedUpCurve; 
        [FormerlySerializedAs("catController")] public Animator catAnimator;
        public CatController catMovementController;
        public Transform catTargetPos;
        public float catMoveOutTime;
        public float standaMoveToDancePosTime = 1.0f;
        public Animator anickaAnimator;
        public float menuStartAfterVictoryTime;
        public Vector2 anickaPosNorm;
        public Vector2 standaPosNorm;
        public LaserSounder LaserSounder;

        public event Action<eGameState> GameStateChanged;

        private Animator _standaAnimator;        
        private FloatStat _standaHealthStat;
        public eGameState GameState => _gameState;
        private eGameState _gameState;
        private StartingGameState _startingGameState;

        private List<GameObject> _allMeteors;
        private List<ParticleSystem> _energySpirits = new List<ParticleSystem>();

        private void Awake()
        {
            startGameButton.onClick.AddListener((() => ChangeGameState(eGameState.IntroAnim)));
            introSequenceDirector.stopped += director => ChangeGameState(eGameState.Game);
            introSequenceDirector.paused += director => ChangeGameState(eGameState.Game);
            _standaAnimator = standa.GetComponentInChildren<Animator>();
            _startingGameState = FindObjectOfType<StartingGameState>();
            winAsteroid.Destroyed.AddListener(OnWinAsteroidDestroyed);
        }
        
        private IEnumerator Start()
        {
            _standaHealthStat = standa.GetComponent<HaveStats>().GetFloatStat(eStatType.Health);
            _standaHealthStat.OnValueChanged += OnStandaHealthChanged;
            ChangeGameState(_startingGameState.startingGameState);
            
            _allMeteors = new List<GameObject>(Resources.FindObjectsOfTypeAll<Destroyable>().Select(d=>d.gameObject)
                .Where(go => go.hideFlags != HideFlags.NotEditable && go.hideFlags != HideFlags.HideAndDontSave));
            foreach (var meteor in _allMeteors)
            {
                for (int i = 0; i < meteor.transform.childCount; i++)
                {
                    var child = meteor.transform.GetChild(i);
                    if (child.CompareTag("EnergySpirit"))
                    {
                        _energySpirits.Add(child.GetComponent<ParticleSystem>());
                    }
                }
            }

            yield return null;
//            yield return new WaitForSeconds(1.0f);
//            ChangeGameState(eGameState.Win);
        }        

        private void OnWinAsteroidDestroyed()
        {
            ChangeGameState(eGameState.Win);
        }

        public void IntroAnimFinished()
        {
            ChangeGameState(eGameState.Game);
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
                    standa.GetComponent<RainbowShooter>().enabled = true;
                    backGroundMusic.Play();
                    gameEnableObjects.ForEach(g => g.SetActive(true));
                    _standaAnimator.SetTrigger("Riding");
                    break;
                case eGameState.Death:
                    TurnOffMovement();
                    TurnOffShooting();
                    TurnOffCameraShake();
                    standa.GetComponentInChildren<Animator>().SetTrigger("Died");
                    StartCoroutine(DeathSequence());
                    break;
                case eGameState.Win:
                    StartCoroutine(WinSequence());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
            
            if (gameState != _gameState)
                GameStateChanged?.Invoke(gameState);

            _gameState = gameState;
        }

        private void TurnOffCameraShake()
        {
            camera.GetComponent<ShakeBehaviour>().enabled = false;
        }

        private void TurnOffMovement()
        {
            catMovementController.enabled = false;
            standa.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            standa.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        private void TurnOffShooting()
        {
            standa.GetComponent<RainbowShooter>().enabled = false;
            LaserSounder.Stop();
        }
        
        private IEnumerator WinSequence()
        {
            TurnOffCameraShake();
            TurnOffShooting();
            TurnOffMovement();
            
            var speedDownCurrentTime = 0f;
            while (speedDownCurrentTime < winAsteroidSpeedownTime)
            {
                speedDownCurrentTime += Time.unscaledDeltaTime;
                Time.timeScale = speedUpCurve.Evaluate(speedDownCurrentTime / winAsteroidSpeedownTime);

                yield return null;
            }

            Time.timeScale = 1f;
            
            // cat goes away
            StartCoroutine(CatWinMoveOutAnim());
            
            // all meteors explode
            foreach (var meteor in _allMeteors)
            {
                if (meteor == null)
                    continue;

                var destr = meteor.GetComponent<Destroyable>();
                if (destr.gameObject.activeInHierarchy)
                    meteor.GetComponent<Destroyable>().Die();
            }
            
            // disable all energy spirits
            foreach (var energySpirit in _energySpirits)
            {
                if (energySpirit != null)
                    energySpirit.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            // anicka gets to her dance position (just plays animation)
            var worldPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width * anickaPosNorm.x, Screen.height * anickaPosNorm.y, 0));
            worldPoint.z = 0;
            anickaAnimator.transform.parent.gameObject.SetActive(true);
            anickaAnimator.transform.parent.position = worldPoint;
            anickaAnimator.SetTrigger("VictoryCome");

            // standa from any place gets to his dance position in x secs
            StartCoroutine(StandaMoveToDancePos(standaMoveToDancePosTime));

            yield return new WaitForSeconds(standaMoveToDancePosTime);
            
            // both start their victory dance
            _standaAnimator.SetTrigger("VictoryDance");
            
            // after y secs standa menu appears and we get to start menu
            yield return new WaitForSeconds(menuStartAfterVictoryTime);
            
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            startGameButton.gameObject.SetActive(true);
        }
        
        private IEnumerator DeathSequence()
        {
            yield return StartCoroutine(ZoomCameraOnStandaFace(deathCameraZoomTime));

//            var crossStartWS = camera.ScreenToWorldPoint(crossStart.localPosition);
//            crossStartWS = new Vector3(crossStartWS.x, crossStartWS.y, 0);
//            var crossEndWS = camera.ScreenToWorldPoint(crossEnd.localPosition);
//            crossEndWS = new Vector3(crossEndWS.x, crossEndWS.y, 0);

//            var rectWorldStart = RectTransformToScreenSpace(crossStart);
//            Vector3 crossStartWS = RectTransformToScreenSpace(crossStart).min;
            Vector3 crossStartWS;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(crossStart, crossStart.localPosition, camera,
                out crossStartWS);

//            var rectWorldEnd = RectTransformToScreenSpace(crossStart);
//            Vector3 crossEndWS = RectTransformToScreenSpace(crossEnd).min;;
            Vector3 crossEndWS;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(crossEnd, crossEnd.localPosition, camera,
                out crossEndWS);

            _startingGameState.startingGameState = eGameState.Game;
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            startGameButton.gameObject.SetActive(true);
            
            cross.SetPositions(new [] { crossStartWS, crossEndWS});
            //cross.gameObject.SetActive(true);
        }

        private IEnumerator CatWinMoveOutAnim()
        {
            catAnimator.enabled = false;

            var catTrans = catAnimator.transform;
            
            var targetTime = Time.time + catMoveOutTime;
            var startPos = catTrans.position;
            var startScale = catTrans.localScale;
            float elapsed = 0;
            
            while (Time.time < targetTime)
            {
                elapsed += Time.deltaTime;
                var progress = elapsed / catMoveOutTime;

                catTrans.position = Vector3.Lerp(startPos, catTargetPos.position, progress);
                var newScale = 1.0f - progress;
                catTrans.localScale = new Vector3(newScale, newScale, newScale);
                
                yield return null;
            }
        }

        private IEnumerator StandaMoveToDancePos(float time)
        {
            var startPos = standa.transform.position;
            
            var worldPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width * standaPosNorm.x, Screen.height * standaPosNorm.y, 0));
            worldPoint.z = 0;
            var endPos = worldPoint;

            var startTime = Time.time;
            var endTime = startTime + time;
            while (Time.time < endTime)
            {
                var progress = 1 - (endTime - Time.time) / time;

                var pos = Vector3.Lerp(startPos, endPos, progress);
                pos.y += Mathf.Sin(progress * Mathf.PI);
                standa.transform.position = pos;

                yield return null;
            }
        }
        
        private IEnumerator ZoomCameraOnStandaFace(float duration)
        {
            float targetTime = Time.time + duration;
            while (Time.time < targetTime)
            {
                var lerpVal = (1 / duration) * Time.deltaTime * deathZoomSpeed;
                var lerpPos = Vector3.Lerp(camera.transform.position, deathTransform.position, lerpVal);
                camera.transform.position = new Vector3(lerpPos.x, lerpPos.y, -10);
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, deathCameraSize, lerpVal);
                
                yield return null;
            }
        }

        private void OnStandaHealthChanged(FloatStat healthStat, float oldVal, float newVal)
        {
            if (newVal <= 0)
            { // player Death
                ChangeGameState(eGameState.Death);
            }
        }
        
        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= (transform.pivot.x * size.x);
            rect.y -= ((1.0f - transform.pivot.y) * size.y);
            return rect;
        }
    }
}