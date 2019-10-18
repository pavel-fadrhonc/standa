using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DoDamageOnCollision : MonoBehaviour
    {
        public float damage;
        public float speedForMaxDamage;
        
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var stats = other.gameObject.GetComponent<HaveStats>();
            if (stats == null)
                return;

            var healthStat = stats.GetFloatStat(eStatType.Health);
            var speed = _rigidbody.velocity.magnitude;
            healthStat.AddValue(-damage * (speed / speedForMaxDamage));
        }
    }
}