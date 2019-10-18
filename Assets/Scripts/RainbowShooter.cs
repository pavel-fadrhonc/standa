using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    [RequireComponent(typeof(HaveStats))]
    [RequireComponent(typeof(EnergyRegenerator))]
    public class RainbowShooter : MonoBehaviour
    {
        public float maxShootDistance = 20f;
        public float damage = 1f;
        public float shootForce = 1f;

        public float shootStartCost = 10f;
        public float shootingCostPerSec = 5f;

        public Transform rainbowStart;
        public RainbowLaser rainbowLaser;

        private FloatStat _energyStat;
        private EnergyRegenerator _energyRegenerator;

        private bool _startedShooting;

        private void Awake()
        {
            _energyStat = GetComponent<HaveStats>().GetFloatStat(eStatType.Energy);
            _energyRegenerator = GetComponent<EnergyRegenerator>();
        }

        private void Update()
        {
            bool fire = Input.GetButton("Fire1");
            bool fireDown = Input.GetButtonDown("Fire1");

            var consumedEnergy = fireDown ? shootStartCost : shootingCostPerSec * Time.deltaTime;

            if (fireDown && _energyStat.Value > consumedEnergy)
                _startedShooting = true;
            
            if (fire && _energyStat.Value > 0 && _startedShooting)
            {
                _energyStat.AddValue(-consumedEnergy);
                _energyRegenerator.enabled = false;
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
                    destroyable.GetComponent<HaveStats>().GetFloatStat(eStatType.Health).AddValue(-damage);
                }
                else
                {
                    rainbowLaser.SetEndPos(rainbowStart.position + rainbowStart.right * maxShootDistance);
                }
            }
            else
            {
                _energyRegenerator.enabled = true;
                _startedShooting = false;
                rainbowLaser.SetEnabled(false);
            }
        }
    }
}