using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MergeItemData mergeItemData;

        private Image SpriteRenderer { get; set; }

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
            Destroy(gameObject);

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
            RefreshSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MergeController.Instance.OnClick(this, eventData.button);
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

                ClearFromMergeItemCell(fromMergeItem);
                
                return true;
            }

            return false;
        }

        private void ClearFromMergeItemCell(MergeItem mergeItem)
        {
            mergeItem.TrySetData(null, true);
        }
    }
}