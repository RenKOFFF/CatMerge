using System;
using System.Collections;
using DG.Tweening;
using GameData;
using Merge.Coins;
using Merge.Generator;
using Unity.VisualScripting;
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

        public bool TryMergeIn(MergeItem itemToMergeIn, Action<int> callback = null)
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

            DOTween.Sequence()
                .Append(itemToMergeIn.transform.DOScale(Vector3.zero, .3f))
                .Append(itemToMergeIn.transform.DOScale(Vector3.one, .3f))
                .SetEase(Ease.OutBounce);

            ClearItemCell();

            callback?.Invoke(itemToMergeIn.MergeItemData.ComplexityLevel);
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

                if (data != null)
                {
                    var sequence = DOTween.Sequence()
                        .Append(transform.DOScale(new Vector3(.9f, .9f), .2f))
                        .Append(transform.DOScale(new Vector3(1.2f, 1.2f), .2f))
                        .Append(transform.DOScale(new Vector3(1, 1), .3f))
                        .SetEase(Ease.OutBounce);

                    //sequence.Append(transform.DOShakeScale(1, .3f));
                }

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
            var slotTransform = transform.parent.parent;
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
            transform.DOLocalMove(Vector3.zero, .3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCount++;

            if (_clickCount > 0)
                StartCoroutine(ClickCountReset());

            if (MergeItemData is GeneratorMergeItemData clickableData)
            {
                DOTween.Sequence()
                    .Append(transform.DOScale(new Vector3(.9f, .9f), .2f))
                    .Append(transform.DOScale(new Vector3(1.2f, 1.2f), .2f))
                    .Append(transform.DOScale(new Vector3(1, 1), .3f))
                    .SetEase(Ease.OutBounce);
                
                clickableData.Spawn();

                MergeController.Instance.OpenItemInfo(this);
                MergeController.Instance.SaveMField();
                return;
            }

            if (MergeItemData is EnergyMergeItemData doubleClickableData)
            {
                if (_clickCount >= 2)
                {
                    doubleClickableData.GiveEnergy();
                    DOTween.Sequence()
                        .Append(transform.DOScale(Vector3.one * 1.2f, .1f))
                        .Append(transform.DOScale(Vector3.zero, .1f))
                        .OnComplete(ClearItemCell);
                }

                MergeController.Instance.OpenItemInfo(this);
                MergeController.Instance.SaveMField();
                return;
            }

            if (MergeItemData is CoinsMergeItemData coinsMergeItemData)
            {
                if (_clickCount >= 2)
                {
                    coinsMergeItemData.GiveCoins();
                    DOTween.Sequence()
                        .Append(transform.DOScale(Vector3.one * 1.2f, .1f))
                        .Append(transform.DOScale(Vector3.zero, .1f))
                        .OnComplete(ClearItemCell);
                }

                MergeController.Instance.OpenItemInfo(this);
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