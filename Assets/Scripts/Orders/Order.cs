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

        public OrderData OrderData { get; private set; }
        [CanBeNull] private UnityAction OnCompleted { get; set; }

        private readonly List<OrderPart> _orderParts = new();
        private Image _background;

        public static Color CompletedOrderColor = new(0.7f, 1, 0.7f);

        public void Initialize(OrderData orderData, [CanBeNull] UnityAction onCompleted = null)
        {
            _background = GetComponent<Image>();
            OrderData = orderData;
            OnCompleted = onCompleted;

            foreach (var partData in OrderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(partData);
                _orderParts.Add(orderPart);
            }

            rewardText.text = $" +{OrderData.RewardMoney}";
            rewardText.autoSizeTextContainer = true;

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
            _background.color = areAllNeededItemsOnFieldNow ? CompletedOrderColor : new Color(1, 1, 1);
        }
    }
}
