using System.Collections.Generic;
using System.Linq;
using GameData;
using Merge;
using UnityEngine;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;

        private OrderData _orderData;

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
            var rewardMoney = 0;

            foreach (var orderPartData in _orderData.Parts)
            {
                var foundMergeItem = MergeController.Instance.FindMergeItemWithData(orderPartData.NeededItem);

                if (foundMergeItem == null)
                    return;

                rewardMoney += Random.Range(1, 4 + 1);
                foundMergeItem.ClearItemCell();
            }

            GameManager.Instance.AddMoney(rewardMoney);

            Destroy(gameObject);
        }

        private void Update()
        {
            var areAllNeededItemsOnFieldNow = true;

            foreach (var orderPartData in _orderData.Parts)
            {
                var foundMergeItem = MergeController.Instance.FindMergeItemWithData(orderPartData.NeededItem);

                if (foundMergeItem == null)
                {
                    areAllNeededItemsOnFieldNow = false;
                    continue;
                }

                foundMergeItem.SetUsedForOrder();
            }

            claimRewardButton.SetActive(areAllNeededItemsOnFieldNow);
        }
    }
}
