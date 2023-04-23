using System.Collections;
using GameData;
using Merge.Coins;
using Merge.Generator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private MergeItemData _mergeItemData;

        private int _clickCount;
        private float _timeBtwClick = .5f;

        private Image SpriteRenderer { get; set; }
        private CanvasGroup CanvasGroup { get; set; }

        public bool IsUsedForOrder { get; private set; }

        public bool IsEmpty => MergeItemData == null;
        public bool IsMoving => transform.localPosition != Vector3.zero;

        public MergeItemData MergeItemData
        {
            get => _mergeItemData;
            private set
            {
                SetUsedForOrder(false);
                _mergeItemData = value;
            }
        }

        public bool TryMergeIn(MergeItem itemToMergeIn)
        {
            if (IsEmpty
                || gameObject == itemToMergeIn.gameObject
                || MergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.MergeItemData = MergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();

            ClearItemCell();

            return true;
        }

        public void SetUsedForOrder(bool isUsed = true)
        {
            IsUsedForOrder = isUsed;
        }

        private bool Equals(MergeItem other)
            => MergeItemData == other.MergeItemData;

        private void RefreshSprite()
        {
            SpriteRenderer.sprite = !IsEmpty ? MergeItemData.sprite : null;
            SpriteRenderer.enabled = !IsEmpty;
        }

        private void Awake()
        {
            SpriteRenderer = GetComponent<Image>();
            CanvasGroup = GetComponent<CanvasGroup>();
            RefreshSprite();
        }

        public bool TrySetData(MergeItemData data, bool forceSet)
        {
            if (forceSet || MergeItemData is null)
            {
                MergeItemData = data;
                RefreshSprite();

                return true;
            }

            return false;
        }

        public bool TrySwapData(MergeItem fromMergeItem)
        {
            if (Equals(fromMergeItem))
                return false;

            var tempMergeItemData = MergeItemData;

            TrySetData(fromMergeItem.MergeItemData, true);
            fromMergeItem.TrySetData(tempMergeItemData, true);

            return true;
        }

        public void ClearItemCell()
        {
            TrySetData(null, true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
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
            ResetItem();
        }

        private void ResetItem()
        {
            CanvasGroup.blocksRaycasts = true;
            transform.localPosition = Vector3.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCount++;

            if (_clickCount > 0)
                StartCoroutine(ClickCountReset());

            if (MergeItemData is GeneratorMergeItemData clickableData)
            {
                clickableData.Spawn();

                MergeController.Instance.SaveMField();
                return;
            }

            if (MergeItemData is EnergyMergeItemData doubleClickableData)
            {
                if (_clickCount >= 2)
                {
                    doubleClickableData.GiveEnergy();
                    ClearItemCell();
                }

                MergeController.Instance.SaveMField();
                return;
            }
            
            if (MergeItemData is CoinsMergeItemData coinsMergeItemData)
            {
                if (_clickCount >= 2)
                {
                    coinsMergeItemData.GiveCoins();
                    ClearItemCell();
                }

                MergeController.Instance.SaveMField();
                return;
            }

            MergeController.Instance.OnClick(this);
        }

        public void Sell()
        {
            GameManager.Instance.AddMoney(MergeItemData.SellPrice);
            ClearItemCell();
        }

        private IEnumerator ClickCountReset()
        {
            while (_clickCount > 0)
            {
                yield return new WaitForSeconds(_timeBtwClick);

                _clickCount = 0;
            }
        }
    }
}
