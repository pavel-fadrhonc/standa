using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddsEnergyOnTrigger : MonoBehaviour
    {
        public float energyAmount;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var haveStats = other.gameObject.GetComponentInParent<HaveStats>();
            if (haveStats == null) return;

            var energyStat = haveStats.GetFloatStat(eStatType.Energy);
            if (energyStat == null) return;
            
            energyStat.AddValue(energyAmount);   
        }
    }
}