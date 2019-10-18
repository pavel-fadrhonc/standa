using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(FloatStat))]
    public class HaveStats : MonoBehaviour
    {
        private Dictionary<eStatType, FloatStat> _floatStats = new Dictionary<eStatType, FloatStat>();

        private void Awake()
        {
            var floatStats = GetComponents<FloatStat>();
            foreach (var floatStat in floatStats)
            {
                _floatStats.Add(floatStat.StatType, floatStat);
            }
        }

        public FloatStat GetFloatStat(eStatType statType)
        {
            return _floatStats.ContainsKey(statType) ? _floatStats[statType] : null;
        }
    }
}