using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(HaveStats))]
    public class FloatStat : MonoBehaviour
    {
        [SerializeField]
        private eStatType _statType;
        public eStatType StatType => _statType;
        
        public event Action<FloatStat, float, float> OnValueChanged;
        
        [SerializeField] private float _maxValue;
        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        private float val;
        public float Value
        {
            get => val;
        }

        private void Awake()
        {
            val = MaxValue;
        }

        public void AddValue(float value)
        {
            var oldVal = Value;
            
            val += value;
            val = Mathf.Clamp(Value, 0, MaxValue);

            if (!Mathf.Approximately(oldVal, Value))
            {
                OnValueChanged?.Invoke(this, oldVal, Value);
            }
                
        }
    }
}