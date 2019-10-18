using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CatController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector2 angleSpan;
        [SerializeField] private Animator standaAnimator;
        
        private Rigidbody2D _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            
            // rotate first

            var angle = transform.eulerAngles.z + hor * rotationSpeed * -1 * Time.fixedDeltaTime;
            if (angle > 180)
                angle -= 360;
            
            angle = Mathf.Clamp(angle, angleSpan.x, angleSpan.y);
            //_rigidbody.MoveRotation(angle);
            _rigidbody.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // move second - evaluate speed based on rotation

            var horizontalSpeedNorm = (1 - Mathf.InverseLerp(angleSpan.x, angleSpan.y, angle)) * 2 - 1;
            var absHor = Mathf.Abs(horizontalSpeedNorm);
            var absVer = Mathf.Abs(ver);

            float directionScale = Mathf.Max(absHor, absVer);

            _rigidbody.velocity = (Vector2.right * horizontalSpeedNorm + Vector2.up * ver).normalized * directionScale * speed;
            //_rigidbody.MovePosition((Vector2) transform.position + (Vector2.right * horizontalSpeedNorm + Vector2.up * ver).normalized * directionScale * speed * Time.fixedDeltaTime);
            
            standaAnimator.SetFloat("Lean", horizontalSpeedNorm);
        }
    }
}