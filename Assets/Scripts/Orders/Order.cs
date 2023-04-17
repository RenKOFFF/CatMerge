using System.Collections.Generic;
using GameData;
using TMPro;
using UnityEngine;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;
        [SerializeField] private TMP_Text rewardText;

        private int Reward { get; set; }
        private OrderData OrderData { get; set; }

        private readonly List<OrderPart> _orderParts = new();

        public void Initialize(OrderData orderData)
        {
            OrderData = orderData;

            foreach (var orderPartData in OrderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(orderPartData);
                _orderParts.Add(orderPart);
            }

            foreach (var orderPartData in _orderParts)
                Reward += orderPartData.GetRewardAmount();

            rewardText.text = $"+{Reward}";
        }

        public void ClaimReward()
        {
            foreach (var orderPartData in _orderParts)
                orderPartData.Complete();

            if (OrderData.RewardItem != null)
                RewardsStack.Instance.AppendReward(OrderData.RewardItem);

            GameManager.Instance.AddMoney(Reward);

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
