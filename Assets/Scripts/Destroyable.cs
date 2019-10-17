using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Destroyable : MonoBehaviour
    {
        public int health;
        public float exploadingPartsForce = 20f;

        public GameObject destroyPartsParent;

        private Rigidbody2D _rigidbody2D;
        
        private List<Rigidbody2D> _destroyPartsRB;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            destroyPartsParent.gameObject.SetActive(false);
            _destroyPartsRB = new List<Rigidbody2D>(destroyPartsParent.GetComponentsInChildren<Rigidbody2D>());
        }

        public void DoDamage(float damage)
        {
            health -= (int) damage;

            if (health < 0)
            {
                Die();    
            }
        }

        public void ApplyForce(Vector3 force)
        {
            _rigidbody2D.AddForce(force);
        }

        private void Die()
        {
            destroyPartsParent.gameObject.SetActive(true);
            
            foreach (var dp in _destroyPartsRB)
            {
                dp.transform.SetParent(null);
                dp.AddForce((dp.transform.position - transform.position).normalized * exploadingPartsForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }
    }
}