using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveWithSpeed : MonoBehaviour
    {
        public Vector3 direction;
        public float speed;

        private Vector3 _dirNorm;

        private void Awake()
        {
            _dirNorm = direction.normalized;
        }

        private void Update()
        {
            transform.position += Time.deltaTime * speed * _dirNorm;
        }
    }
}