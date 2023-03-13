using UnityEngine;
using UnityEngine.EventSystems;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MergeItemData mergeItemData;

        private SpriteRenderer SpriteRenderer { get; set; }

        public bool TryMergeIn(MergeItem itemToMergeIn)
        {
            if (gameObject == itemToMergeIn.gameObject
                || mergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.mergeItemData = mergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();
            Destroy(gameObject);

            return true;
        }

        private bool Equals(MergeItem other)
            => mergeItemData == other.mergeItemData;

        private void RefreshSprite()
        {
            SpriteRenderer.sprite = mergeItemData.sprite;
        }

        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            RefreshSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MergeController.Instance.OnClick(this, eventData.button);
        }
    }
}
