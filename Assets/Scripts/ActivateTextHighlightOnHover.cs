using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class ActivateTextHighlightOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Material normalMaterial;
        public Material hoverMaterial;
        private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _textMesh.fontSharedMaterial = hoverMaterial;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _textMesh.fontSharedMaterial = normalMaterial;
        }
    }
}