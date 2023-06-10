using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameData;
using Merge;
using Newtonsoft.Json;
using Orders.Data;
using SaveSystem;
using SaveSystem.SaveData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private double timeToGenerateOrderInSeconds = 3;
        [SerializeField] private int maxActiveOrdersCount = 5;
        [SerializeField] private int ordersNeededToCompleteLevelCount = 20;
        [SerializeField] private Order orderPrefab;
        [SerializeField] private Transform ordersParent;
        [SerializeField] private LevelCompletedHandler levelCompletedPanelPrefab;
        [SerializeField] private MainMenu.MainMenu menuCanvas;

        private int _completedOrdersCount;

        public List<Order> ActiveOrders { get; private set; } = new();
        private DateTime NextOrderGenerationTime { get; set; }

        public int CompletedOrdersCount
        {
            get => _completedOrdersCount;
            private set
            {
                _completedOrdersCount = value;
                ComoletedOrdersChanged?.Invoke(_completedOrdersCount);
            }
        }

        public int OrdersNeededToCompleteLevelCount => ordersNeededToCompleteLevelCount;

        public static OrderManager Instance;
        public event Action LevelCompleted;
        public event Action<int> ComoletedOrdersChanged;

        private void OnEnable()
        {
            GameManager.Instance.LevelChanged += OnLevelChanged;
            LevelCompleted += GameManager.Instance.OpenNextLevelAndCloseCurrent;
        }

        private void OnDisable()
        {
            GameManager.Instance.LevelChanged -= OnLevelChanged;
            LevelCompleted -= GameManager.Instance.OpenNextLevelAndCloseCurrent;
        }

        private void OnLevelChanged(int _)
        {
            LoadOrdersOnCurrentLevel();
        }

        private void GenerateOrder()
        {
            if (ActiveOrders.Count >= maxActiveOrdersCount)
                return;

            var partsCount = GetOrderPartsCount();
            var itemsForPartsData = new List<MergeItemData>();

            var availableMergeItems = GameDataHelper.AllMergeItems
                .Where(i =>
                {
                    var unlockedComplexityLevel = MergeController.Instance.GetUnlockedComplexityLevel(i);
                    var maxItemLevel = Math.Min(unlockedComplexityLevel - partsCount + 2, 7);
                    var minItemLevel = Math.Max(1, maxItemLevel - 2);
                    var complexityLevel = i.ComplexityLevel;

                    return complexityLevel >= minItemLevel && complexityLevel <= maxItemLevel;
                })
                .ToList();

            for (var i = 0; i < partsCount; i++)
            {
                if (availableMergeItems.Count == 0)
                    break;

                var randomIndex = Random.Range(0, availableMergeItems.Count);
                var randomItem = availableMergeItems[randomIndex];

                itemsForPartsData.Add(randomItem);
                availableMergeItems.RemoveAt(randomIndex);
            }

            var orderIncludesRewardItemProbability = itemsForPartsData.Average(i => i.ComplexityLevel) * partsCount * 0.08;
            var isRewardItemIncluded = Random.Range(0f, 1) < orderIncludesRewardItemProbability;
            MergeItemData rewardItem = null;

            if (isRewardItemIncluded)
            {
                var availableRewardItems = GameDataHelper.AllRewardItems.ToList();
                var randomIndex = Random.Range(0, availableRewardItems.Count);
                rewardItem = availableRewardItems[randomIndex];
            }

            var orderData = CreateSpecificOrderData(rewardItem, true, itemsForPartsData);
            SpawnOrder(orderData, false);
        }

        private static int GetOrderPartsCount()
        {
            var chanceInPercent = Random.Range(0, 100);
            var currentLevel = GameManager.Instance.CurrentLevel;

            return currentLevel switch
            {
                1 => 1,
                2 or 3 => chanceInPercent switch
                {
                    < 5 => 3,
                    < 30 => 2,
                    _ => 1
                },
                _ => chanceInPercent switch
                {
                    < 30 => 3,
                    < 60 => 2,
                    _ => 1
                }
            };
        }

        private static OrderData CreateSpecificOrderData(
            MergeItemData rewardItem,
            bool containsRewardMoney,
            List<MergeItemData> itemsForPartsData)
        {
            var orderData = new OrderData(rewardItem, containsRewardMoney);

            foreach (var itemData in itemsForPartsData)
            {
                var orderPartData = new OrderPartData(itemData);
                orderData.AddPart(orderPartData);
            }

            return orderData;
        }

        private void SpawnOrder(OrderData orderData, bool isLoadSpawn)
        {
            void OnOrderCompleted()
            {
                if (orderData.ContainsRewardItem)
                    RewardsStack.Instance.AppendReward(orderData.RewardItem);

                if (orderData.ContainsRewardMoney)
                    GameManager.Instance.AddMoney(orderData.RewardMoney);

                CompletedOrdersCount++;

                if (CompletedOrdersCount == ordersNeededToCompleteLevelCount)
                {
                    var canvas = GameObject.FindGameObjectWithTag(GameConstants.Tags.Canvas);
                    var levelCompletedPanel = Instantiate(levelCompletedPanelPrefab, canvas.transform, false);

                    levelCompletedPanel.Initialize(() =>
                    {
                        menuCanvas.ShowMenu();
                        Destroy(levelCompletedPanel.gameObject);
                    });
                    LevelCompleted?.Invoke();
                }
            }

            var order = Instantiate(orderPrefab, ordersParent, false);
            order.Initialize(orderData, OnOrderCompleted);
            ActiveOrders.Add(order);

            if (!isLoadSpawn)
                SaveManager.Instance.Save(
                    new OrdersSaveData(Instance),
                    GameManager.Instance.CurrentLevel.ToString());
        }

        private void SetNewOrderGenerationTime()
        {
            NextOrderGenerationTime = DateTime.UtcNow + TimeSpan.FromSeconds(timeToGenerateOrderInSeconds);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetNewOrderGenerationTime();
            LoadOrdersOnCurrentLevel();
        }

        private void LoadOrdersOnCurrentLevel()
        {
            var ordersSaveData = SaveManager.Instance.LoadOrDefault(
                new OrdersSaveData(),
                GameManager.Instance.CurrentLevel.ToString());

            var rewardDict =
                JsonConvert.DeserializeObject<Dictionary<int, string>>(ordersSaveData.rewardDictJSonFormat);
            var hasMoneyDict =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(ordersSaveData.hasMoneyDictJSonFormat);
            var partsDict =
                JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, string>>>(ordersSaveData
                    .partsDictJSonFormat);

            CompletedOrdersCount = ordersSaveData.CompletedOrdersCount;

            foreach (var currentActiveOrder in ActiveOrders)
            {
                if (currentActiveOrder)
                    Destroy(currentActiveOrder.gameObject);
            }

            ActiveOrders.Clear();

            for (int i = 0; i < rewardDict.Keys.Count; i++)
            {
                MergeItemData rewardItem = FindMergeItemDataOnResourcesByDict(rewardDict, i);

                List<MergeItemData> partsList = new();
                foreach (var partName in partsDict[i].Values)
                {
                    MergeItemData partMergeItemData = FindMergeItemDataOnResourcesByName(partName);
                    partsList.Add(partMergeItemData);
                }

                var loadedOrderData = CreateSpecificOrderData(rewardItem, hasMoneyDict[i], partsList);
                SpawnOrder(loadedOrderData, true);
            }

            MergeItemData FindMergeItemDataOnResourcesByDict(Dictionary<int, string> dictionary, int i)
            {
                if (dictionary.TryGetValue(i, out var dataName))
                    return FindMergeItemDataOnResourcesByName(dataName);

                return null;
            }

            MergeItemData FindMergeItemDataOnResourcesByName(string dataName)
            {
                string directory = dataName.Split("_")[0];
                var mergeItemData = Resources.Load<MergeItemData>($"MergeItems/{directory}/{dataName}");
                return mergeItemData;
            }
        }

        private void UpdateActiveOrders()
        {
            ActiveOrders = ActiveOrders
                .Where(o => o != null && o.gameObject != null)
                .OrderByDescending(o => o.CompletedProgress)
                .ToList();

            foreach (var order in ActiveOrders)
                order.transform.SetAsLastSibling();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentLevel == 0)
                return;

            UpdateActiveOrders();

            if (!NextOrderGenerationTime.IsPassed())
                return;

            GenerateOrder();
            SetNewOrderGenerationTime();
        }
    }
}
