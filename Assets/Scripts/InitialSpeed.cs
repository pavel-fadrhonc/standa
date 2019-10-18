using UnityEngine;

namespace DefaultNamespace
{
    public class InitialSpeed : MonoBehaviour
    {
        public enum InitialSpeedMode
        {
            Constant,
            RandomBetweenNumbers
        }

        public InitialSpeedMode initialSpeedMode = InitialSpeedMode.Constant;
        public Vector2 randomRange = new Vector2();
        public float speed;
        public Vector3 direction;
        public ForceMode2D forceMode;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            float force = speed;
            if (initialSpeedMode == InitialSpeedMode.RandomBetweenNumbers)
                force = Random.Range(randomRange.x, randomRange.y);
            
            _rigidbody.AddForce(direction.normalized * force, forceMode);
        }
    }
}