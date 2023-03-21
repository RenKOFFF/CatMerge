using System.Collections.Generic;
using System.Linq;
using Merge;
using UnityEngine;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;

        private bool AreAllNeededItemsOnFieldNow => _usedItems.Count == _orderData.Parts.Count;

        private OrderData _orderData;
        private readonly List<MergeItem> _usedItems = new();

        public void Initialize(OrderData orderData)
        {
            _orderData = orderData;

            foreach (var orderPartData in orderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(orderPartData);
            }
        }

        public void ClaimReward()
        {
            foreach (var mergeItem in _usedItems)
                mergeItem.ClearItemCell();

            Destroy(gameObject);
        }

        private void Update()
        {
            UpdateUsedItems();

            if (AreAllNeededItemsOnFieldNow)
                return;

            foreach (var orderPartData in _orderData.Parts)
            {
                var foundMergeItem = MergeController.Instance.FindMergeItemWithData(orderPartData.NeededItem);

                if (foundMergeItem == null)
                    break;

                foundMergeItem.UseForOrder();
                _usedItems.Add(foundMergeItem);
            }

            claimRewardButton.SetActive(AreAllNeededItemsOnFieldNow);
        }

        private void UpdateUsedItems()
        {
            foreach (var mergeItem in _usedItems.ToList().Where(mergeItem => mergeItem.IsEmpty))
                _usedItems.Remove(mergeItem);
        }
    }
}
