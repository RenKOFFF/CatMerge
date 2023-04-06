﻿using Merge;
using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class OrderPart : MonoBehaviour
    {
        [SerializeField] private Image orderPartImage;
        [SerializeField] private GameObject isItemOnFieldFlag;

        public bool IsItemOnField { get; set; }

        private OrderPartData _orderPartData;

        public void Initialize(OrderPartData orderPartData)
        {
            _orderPartData = orderPartData;
            orderPartImage.sprite = _orderPartData.NeededItem.sprite;
        }

        public int ClaimReward()
        {
            var foundMergeItem = MergeController.Instance.FindMergeItemWithData(_orderPartData.NeededItem);
            var reward = Random.Range(1, 4 + 1) * foundMergeItem.MergeItemData.ComplexityLevel;

            foundMergeItem.ClearItemCell();
            Destroy(gameObject);
            UseForOrderAllNeededItemsOnField(false);

            return reward;
        }

        private void UseForOrderAllNeededItemsOnField(bool isUsed = true)
        {
            var foundMergeItems = MergeController.Instance.FindMergeItemsWithData(_orderPartData.NeededItem);

            foreach (var item in foundMergeItems)
                item.SetUsedForOrder(isUsed);
        }

        private void Update()
        {
            var foundMergeItem = MergeController.Instance.FindMergeItemWithData(_orderPartData.NeededItem);

            IsItemOnField = foundMergeItem != null;
            isItemOnFieldFlag.SetActive(IsItemOnField);

            if (IsItemOnField)
                UseForOrderAllNeededItemsOnField();
        }
    }
}
