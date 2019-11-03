using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RainbowLaser : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem HitDebrisPS;
        [SerializeField]
        private ParticleSystem HitGlowPS;

        private LineRenderer _rainbowLineRenderer;
        private Vector3[] _lineRendPos = new Vector3[2];

        private void Awake()
        {
            _rainbowLineRenderer = GetComponentInChildren<LineRenderer>();
            SetEnabled(false);
        }

        private void Update()
        {
            
        }

        public void SetEnabled(bool enabled)
        {
            _rainbowLineRenderer.enabled = enabled;
            if (enabled)
            {
                HitDebrisPS.Play();
                HitGlowPS.Play();
            }
            else
            {
                HitDebrisPS.Stop();
                HitGlowPS.Stop();
            }
        }
        
        public void SetStartPos(Vector3 pos)
        {
            _lineRendPos[0] = pos;
            
            _rainbowLineRenderer.SetPositions(_lineRendPos);
        }

        public void SetEndPos(Vector3 endPos)
        {
            _lineRendPos[1] = endPos;
            HitDebrisPS.transform.position = endPos;
            HitGlowPS.transform.position = endPos;
            
            _rainbowLineRenderer.SetPositions(_lineRendPos);
        }
    }
}