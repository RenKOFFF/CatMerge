using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameData;
using Merge;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private Order orderPrefab;
        [SerializeField] private Transform ordersParent;
        [SerializeField] private double timeToGenerateOrderInSeconds;
        [SerializeField] private int maxActiveOrdersCount;

        private const double OrderIncludesRewardItemProbability = 0.5;
        private const double OrderIncludesRewardMoneyProbability = 0.5;

        private List<GameObject> ActiveOrders { get; set; } = new();
        private DateTime NextOrderGenerationTime { get; set; }

        private void GenerateOrder()
        {
            ActiveOrders = ActiveOrders
                .Where(o => o != null)
                .ToList();

            if (ActiveOrders.Count >= maxActiveOrdersCount)
                return;

            var availableMergeItems = GameDataHelper.AllMergeItems.ToList();
            var partsAmount = Random.Range(1, 3 + 1);

            var isRewardItemIncluded = Random.Range(0f, 1) < OrderIncludesRewardItemProbability;
            var containsRewardMoney = Random.Range(0f, 1) < OrderIncludesRewardMoneyProbability;
            MergeItemData rewardItem = null;

            if (isRewardItemIncluded)
            {
                var availableRewardItems = GameDataHelper.AllItems.ToList();
                var randomIndex = Random.Range(0, availableRewardItems.Count);
                rewardItem = availableRewardItems[randomIndex];
            }

            var orderData = new OrderData(rewardItem, containsRewardMoney);

            for (var i = 0; i < partsAmount; i++)
            {
                if (availableMergeItems.Count == 0)
                    break;

                var randomIndex = Random.Range(0, availableMergeItems.Count);
                var randomItem = availableMergeItems[randomIndex];
                availableMergeItems.RemoveAt(randomIndex);

                var orderPartData = new OrderPartData(randomItem);
                orderData.AddPart(orderPartData);
            }

            SpawnOrder(orderData);
        }

        private void SpawnOrder(OrderData orderData)
        {
            var order = Instantiate(orderPrefab, ordersParent, false);
            order.Initialize(orderData);
            ActiveOrders.Add(order.gameObject);
        }

        private void SetNewOrderGenerationTime()
        {
            NextOrderGenerationTime = DateTime.UtcNow + TimeSpan.FromSeconds(timeToGenerateOrderInSeconds);
        }

        private void Start()
        {
            SetNewOrderGenerationTime();
        }

        private void Update()
        {
            if (!NextOrderGenerationTime.IsPassed())
                return;

            GenerateOrder();
            SetNewOrderGenerationTime();
        }
    }
}
