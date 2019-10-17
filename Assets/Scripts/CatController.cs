using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CatController : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        private Rigidbody2D _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");

            if (Mathf.Approximately(hor, 0) && Mathf.Approximately(ver, 0))
                return;

            var absHor = Mathf.Abs(hor);
            var absVer = Mathf.Abs(ver);

            float directionScale = 0;

            if (absHor > absVer)
                directionScale = absHor * Mathf.Sign(hor);
            else
                directionScale = absVer * Mathf.Sign(ver);

            directionScale = Mathf.Max(absHor, absVer);
            
            _rigidbody.velocity = (Vector2.right * hor + Vector2.up * ver).normalized * directionScale * speed * Time.deltaTime;
        }
    }
}