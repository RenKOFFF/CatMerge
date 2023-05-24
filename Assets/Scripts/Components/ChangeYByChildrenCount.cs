using UnityEngine;

namespace Components
{
    public class ChangeYByChildrenCount : MonoBehaviour
    {
        [SerializeField] private float firstY;
        [SerializeField] private float secondY;
        [SerializeField] private int changeWhenChildrenCountMoreThan;

        private RectTransform _rectTransform;

        private void UpdateY()
        {
            var y = transform.childCount > changeWhenChildrenCountMoreThan
                ? secondY
                : firstY;

            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, y);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateY();
        }
    }
}
