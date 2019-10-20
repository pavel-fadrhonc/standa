using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyableDisabler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Meteor"))
                other.gameObject.SetActive(false);
        }
    }
}