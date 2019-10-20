using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ActionOnCollision : MonoBehaviour
    {
        public UnityEvent Collided;
        
        public string tagFilter;
        public LayerMask layerFilter;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((1 << other.gameObject.layer & layerFilter.value) > 0)
            {
                if (tagFilter != "")
                {
                    if (other.gameObject.CompareTag(tagFilter))
                    {
                        Collided?.Invoke();
                    }
                }
                else
                {
                    Collided?.Invoke();
                }
            }
        }
    }
}