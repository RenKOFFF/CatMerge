using UnityEngine;

namespace Components
{
    public class ControlWidthByChildSize : MonoBehaviour
    {
        [SerializeField] private RectTransform child;
        [SerializeField] private float minWidth;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var childWidth = child.rect.width;

            if (childWidth < minWidth)
                return;

            _rectTransform.sizeDelta = new Vector2(childWidth, _rectTransform.rect.height);
        }
    }
}
