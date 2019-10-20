using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyOnTrigger : MonoBehaviour
    {
        public LayerMask layerMask;

        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            yield return null;
            
            if (other != null && ((1 << other.gameObject.layer) & layerMask.value) > 0)
            {
                Destroy(gameObject);
            }
        }
    }
}