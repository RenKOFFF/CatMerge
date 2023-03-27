using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private MergeItemData mergeItemData;

        private Image SpriteRenderer { get; set; }
        private CanvasGroup _canvasGroup;

        public bool IsEmpty => mergeItemData == null;
        public MergeItemData MergeItemData => mergeItemData;

        public bool TryMergeIn(MergeItem itemToMergeIn)
        {
            Debug.Log(gameObject == itemToMergeIn.gameObject);

            if (IsEmpty) return false;

            if (gameObject == itemToMergeIn.gameObject
                || mergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.mergeItemData = mergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();

            ClearItemCell(this);

            return true;
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
            _canvasGroup = GetComponent<CanvasGroup>();
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
            if (mergeItemData is null)
            {
                mergeItemData = fromMergeItem.MergeItemData;
                RefreshSprite();

                ClearItemCell(fromMergeItem);
                
                return true;
            }

            return false;
        }

        private void ClearItemCell(MergeItem mergeItem)
        {
            mergeItem.TrySetData(null, true);
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
            _canvasGroup.blocksRaycasts = true;
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