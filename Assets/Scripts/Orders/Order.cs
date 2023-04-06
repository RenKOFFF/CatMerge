using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;

        private readonly List<OrderPart> _orderParts = new();

        public void Initialize(OrderData orderData)
        {
            foreach (var orderPartData in orderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(orderPartData);
                _orderParts.Add(orderPart);
            }
        }

        public void ClaimReward()
        {
            var rewardMoney = 0;

            foreach (var orderPartData in _orderParts)
                rewardMoney += orderPartData.ClaimReward();

            GameManager.Instance.AddMoney(rewardMoney);

            Destroy(gameObject);
        }

        private void Update()
        {
            var areAllNeededItemsOnFieldNow = true;

            foreach (var orderPartData in _orderParts)
                areAllNeededItemsOnFieldNow &= orderPartData.IsItemOnField;

            claimRewardButton.SetActive(areAllNeededItemsOnFieldNow);
        }
    }
}
