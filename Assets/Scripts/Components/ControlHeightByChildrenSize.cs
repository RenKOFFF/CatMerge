using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class ControlHeightByChildrenSize : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> children;
        [SerializeField] private float offset;

        private RectTransform _rectTransform;

        private void UpdateHeight()
        {
            var sumHeight = children.Sum(child => child.rect.height) + offset;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, sumHeight);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateHeight();
        }
    }
}
