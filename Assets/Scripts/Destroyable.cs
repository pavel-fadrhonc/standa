using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(HaveStats))]
    [RequireComponent(typeof(AudioSource))]
    public class Destroyable : MonoBehaviour
    {
        public float exploadingPartsForce = 20f;
        public List<SpriteRenderer> cracks = new List<SpriteRenderer>();
        public List<AudioClip> crackAudio = new List<AudioClip>();
        public float crackVolume = 1;
        
        public GameObject destroyPartsParent;
        public UnityEvent Destroyed;

        private Rigidbody2D _rigidbody2D;
        
        private List<Rigidbody2D> _destroyPartsRB;
        
        private int _cracksEnabled;
        private int _totalCracks;

        private FloatStat _healthStat;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            cracks.ForEach(c=>c.gameObject.SetActive(false));
            _cracksEnabled = 0;
            _totalCracks = cracks.Count + 1;
            
            destroyPartsParent.gameObject.SetActive(false);
            _destroyPartsRB = new List<Rigidbody2D>(destroyPartsParent.GetComponentsInChildren<Rigidbody2D>());
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _healthStat = GetComponent<HaveStats>().GetFloatStat(eStatType.Health);
            _healthStat.OnValueChanged += HealthStatOnOnValueChanged;
        }

        private void HealthStatOnOnValueChanged(FloatStat stat, float oldVal, float newVal)
        {
            if (Mathf.Approximately(newVal, 0))
                Die();
            else
                RefreshCracks();
        }

        private void RefreshCracks()
        {
            var shouldHaveCracks = (int) Mathf.Floor((_healthStat.MaxValue - _healthStat.Value) / (_healthStat.MaxValue / _totalCracks));
            if (_cracksEnabled < shouldHaveCracks)
            {
                for (int i = 0; i < shouldHaveCracks - _cracksEnabled; i++)
                {
                    var inactiveCracks = cracks.Where(c => c.gameObject.activeSelf == false).ToList();
                    inactiveCracks[Random.Range(0, inactiveCracks.Count)].gameObject.SetActive(true);
                }

                _cracksEnabled = shouldHaveCracks;

                _audioSource.Stop();
                _audioSource.clip = crackAudio[_cracksEnabled-1];
                _audioSource.volume = crackVolume;
                _audioSource.Play();
            }            
        }

        public void ApplyForce(Vector3 force)
        {
            _rigidbody2D.AddForce(force);
        }

        public void Die()
        {
            destroyPartsParent.gameObject.SetActive(true);
            if (_destroyPartsRB != null)
            {
                foreach (var dp in _destroyPartsRB)
                {
                    dp.transform.SetParent(null);
                    dp.AddForce((dp.transform.position - transform.position).normalized * exploadingPartsForce, ForceMode2D.Impulse);
                }
            }
            
            Destroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}