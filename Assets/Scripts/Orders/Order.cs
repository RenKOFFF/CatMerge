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

        private readonly List<OrderPart> _orderParts = new();
        private int _reward;

        public void Initialize(OrderData orderData)
        {
            foreach (var orderPartData in orderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(orderPartData);
                _orderParts.Add(orderPart);
            }

            foreach (var orderPartData in _orderParts)
                _reward += orderPartData.GetRewardAmount();

            rewardText.text = $"+{_reward}";
        }

        public void ClaimReward()
        {
            foreach (var orderPartData in _orderParts)
                orderPartData.Complete();

            GameManager.Instance.AddMoney(_reward);

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
