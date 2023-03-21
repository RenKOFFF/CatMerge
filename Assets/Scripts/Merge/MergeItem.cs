using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private MergeItemData mergeItemData;

        private Image SpriteRenderer { get; set; }
        private CanvasGroup CanvasGroup { get; set; }

        public bool IsUsedForOrder { get; private set; }

        public bool IsEmpty => mergeItemData == null;
        public MergeItemData MergeItemData => mergeItemData;

        public bool TryMergeIn(MergeItem itemToMergeIn)
        {
            if (IsEmpty
                || gameObject == itemToMergeIn.gameObject
                || mergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.mergeItemData = mergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();

            ClearItemCell();

            return true;
        }

        public void UseForOrder(bool isUsed = true)
        {
            IsUsedForOrder = isUsed;
        }

        private bool Equals(MergeItem other)
            => mergeItemData == other.mergeItemData;

        private void RefreshSprite()
        {
            if (IsEmpty)
            {
                SpriteRenderer.sprite = null;
                return;
            }

            SpriteRenderer.sprite = mergeItemData.sprite;
        }

        private void Awake()
        {
            SpriteRenderer = GetComponent<Image>();
            CanvasGroup = GetComponent<CanvasGroup>();
            RefreshSprite();
        }

        public bool TrySetData(MergeItemData data, bool forceSet)
        {
            if (forceSet || mergeItemData is null)
            {
                mergeItemData = data;
                RefreshSprite();

                return true;
            }

            return false;
        }

        public bool TrySwapData(MergeItem fromMergeItem)
        {
            if (Equals(fromMergeItem))
            {
                return false;
            }

            var tempMergeItemData = mergeItemData;

            TrySetData(fromMergeItem.MergeItemData, true);
            fromMergeItem.TrySetData(tempMergeItemData, true);

            //ClearItemCell(fromMergeItem);

            return true;
        }

        public void ClearItemCell()
        {
            TrySetData(null, true);
            UseForOrder(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            var slotTransform = transform.parent;
            slotTransform.SetAsLastSibling();

            MergeController.Instance.OnBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MergeController.Instance.OnDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
            transform.localPosition = Vector3.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (MergeItemData is ClickableMergeItemData clickableData)
            {
                clickableData.Spawn();
            }
        }
    }
}
