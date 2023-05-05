using System.Collections.Generic;
using GameData;
using JetBrains.Annotations;
using Orders.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private Image rewardItemImage;

        private OrderData OrderData { get; set; }
        [CanBeNull] private UnityAction OnCompleted { get; set; }

        private readonly List<OrderPart> _orderParts = new();

        public void Initialize(OrderData orderData, [CanBeNull] UnityAction onCompleted = null)
        {
            OrderData = orderData;
            OnCompleted = onCompleted;

            foreach (var partData in OrderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(partData);
                _orderParts.Add(orderPart);
            }

            rewardText.text = OrderData.ContainsRewardMoney ? $" + {OrderData.RewardMoney}" : "";
            rewardText.gameObject.SetActive(OrderData.ContainsRewardMoney);

            if (OrderData.ContainsRewardItem)
                rewardItemImage.sprite = orderData.RewardItem.sprite;

            rewardItemImage.gameObject.SetActive(OrderData.ContainsRewardItem);
        }

        public void ClaimReward()
        {
            foreach (var orderPartData in _orderParts)
                orderPartData.Complete();

            OnCompleted?.Invoke();
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
