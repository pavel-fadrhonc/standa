using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyWithDelay : MonoBehaviour
    {
        public float delay;
        
        private void OnEnable()
        {
            Destroy(gameObject, delay);
        }
    }
}