using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyableCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Destroyable"))
            {
                // TODO: recycle destroyables
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("NonPlayerColliding"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}