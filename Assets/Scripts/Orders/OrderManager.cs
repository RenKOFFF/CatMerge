using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Merge;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private double timeToGenerateOrderInSeconds;
        [SerializeField] private int maxActiveOrdersCount;

        private List<MergeItemData> AllMergeItems { get; set; } = new();
        private List<Order> ActiveOrders { get; } = new();
        private DateTime NextOrderGenerationTime { get; set; }

        private void GenerateOrder()
        {
            if (ActiveOrders.Count >= maxActiveOrdersCount)
                return;

            var partsAmount = Random.Range(1, 1 + 1);
            var order = new Order();

            for (var i = 0; i < partsAmount; i++)
            {
                var randomItem = GetRandomItem();
                var part = new OrderPart(randomItem, 1);
                order.AddPart(part);
            }

            ActiveOrders.Add(order);
        }

        private MergeItemData GetRandomItem()
        {
            var randomIndex = Random.Range(0, AllMergeItems.Count);
            return AllMergeItems[randomIndex];
        }

        private void SetNewOrderGenerationTime()
        {
            NextOrderGenerationTime = DateTime.UtcNow + TimeSpan.FromSeconds(timeToGenerateOrderInSeconds);
        }

        private void Awake()
        {
            AllMergeItems = Resources.LoadAll<MergeItemData>("MergeItems").ToList();
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
