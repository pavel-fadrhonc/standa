using UnityEngine;

namespace DefaultNamespace
{
    public class InitialRotation : MonoBehaviour
    {
        public enum InitialRotationMode
        {
            Constant,
            RandomBetweenNumbers
        }

        public InitialRotationMode initialSpeedMode = InitialRotationMode.Constant;
        public Vector2 randomRange = new Vector2();
        public float torque;
        public ForceMode2D forceMode;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            float torque = this.torque;
            if (initialSpeedMode == InitialRotationMode.RandomBetweenNumbers)
                torque = Random.Range(randomRange.x, randomRange.y);
            
            _rigidbody.AddTorque(torque, forceMode);
        }
    }
}