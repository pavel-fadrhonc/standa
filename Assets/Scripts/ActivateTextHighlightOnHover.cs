using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ActivateTextHighlightOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Material normalMaterial;
        public Material hoverMaterial;
        private TextMeshProUGUI _textMesh;

        private AudioSource _Audio; 

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
            _Audio = GetComponent<AudioSource>();
            GetComponentInParent<Button>().onClick.AddListener(() => _Audio.Play());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _textMesh.fontSharedMaterial = hoverMaterial;
            _Audio.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _textMesh.fontSharedMaterial = normalMaterial;
        }
    }
}