using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ObjectGenerator : MonoBehaviour
    {
        public GameObject objectPrefab;
        public Vector2 objectCountSpan;
        public Vector2 timeIntervalSpan;
        
        private BoxCollider2D _boxCollider2D;
        private float _nextTimeSpan;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (Time.time > _nextTimeSpan)
            {
                // spawn

                _nextTimeSpan = Time.time + Random.Range(timeIntervalSpan.x, timeIntervalSpan.y);
            }
        }
        
        
    }
}