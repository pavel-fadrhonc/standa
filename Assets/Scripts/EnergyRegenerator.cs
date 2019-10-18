using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(HaveStats))]
    public class EnergyRegenerator : MonoBehaviour
    {
        public float energyRegenPerSec;

        private FloatStat _energyStat;

        private void Start()
        {
            _energyStat = GetComponent<HaveStats>().GetFloatStat(eStatType.Energy);
        }

        private void Update()
        {
            _energyStat.AddValue(energyRegenPerSec * Time.deltaTime);
        }
    }
}