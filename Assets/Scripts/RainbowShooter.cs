using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class RainbowShooter : MonoBehaviour
    {
        public float maxShootDistance = 20f;
        public float damage = 1f;
        public float shootForce = 1f;
        
        public Transform rainbowStart;
        public RainbowLaser rainbowLaser;
        
        private void Update()
        {
            bool fire = Input.GetButton("Fire1");

            if (fire)
            {
                rainbowLaser.SetEnabled(true);
                
                var hit = Physics2D.Raycast(rainbowStart.position, rainbowStart.right, maxShootDistance,
                    1 << LayerMask.NameToLayer("Destroyable"));
                
                Debug.DrawLine(rainbowStart.position, rainbowStart.position + rainbowStart.right * maxShootDistance );

                rainbowLaser.SetStartPos(rainbowStart.position);
                
                if (hit.collider != null)
                {
                    rainbowLaser.SetEndPos(hit.point);
                    var destroyable = hit.collider.gameObject.GetComponent<Destroyable>();
                    destroyable.ApplyForce((hit.collider.transform.position - rainbowStart.position).normalized * shootForce);
                    destroyable.DoDamage(damage);
                }
                else
                {
                    rainbowLaser.SetEndPos(rainbowStart.position + rainbowStart.right * maxShootDistance);
                }
            }
            else
            {
                rainbowLaser.SetEnabled(false);
            }
        }
    }
}