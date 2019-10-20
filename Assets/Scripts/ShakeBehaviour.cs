using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ShakeBehaviour : MonoBehaviour
    {
        // Transform of the GameObject you want to shake
        private Transform transform;

        // A measure of magnitude for the shake. Tweak based on your preference
        public float shakeMagnitude = 0.2f;
 
        // A measure of how quickly the shake effect should evaporate
        private float dampingSpeed = 1.0f;
 
        // The initial position of the GameObject
        Vector3 initialPosition;
        
        void Awake()
        {
            if (transform == null)
            {
                transform = GetComponent(typeof(Transform)) as Transform;
            }
        }
        
        void OnEnable()
        {
            initialPosition = transform.localPosition;
        }
        
        void Update()
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
        }

        private void OnDisable()
        {
            transform.localPosition = initialPosition;
        }
    }
}