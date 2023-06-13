using System;
using System.Collections.Generic;
using GameData;
using Merge;
using Newtonsoft.Json;
using SaveSystem;
using SaveSystem.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class RewardsStack : MonoBehaviour
    {
        [SerializeField] private Image lastRewardImage;
        [SerializeField] private TMP_Text rewardsCountText;

        public static RewardsStack Instance { get; private set; }

        public Stack<MergeItemData> Rewards { get; private set; } = new();

        public void AppendReward(MergeItemData reward)
        {
            Rewards.Push(reward);
            UpdateSprite();
            gameObject.SetActive(true);
            
            SaveManager.Instance.Save(
                new OrdersSaveData(OrderManager.Instance),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");
        }

        public void ClaimReward()
        {
            var reward = Rewards.Peek();
            var emptyCellIndex = MergeController.GetEmptyCellIndex();

            if (emptyCellIndex == -1)
                return;

            MergeController.Instance.MergeCells[emptyCellIndex]
                .GetComponentInChildren<MergeItem>()
                .TrySetData(reward, false);

            Rewards.Pop();
            UpdateSprite();
            
            SaveManager.Instance.Save(
                new OrdersSaveData(OrderManager.Instance),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");
        }

        private void ClearStack() => Rewards.Clear();

        private void UpdateSprite()
        {
            if (Rewards.Count > 0)
                lastRewardImage.sprite = Rewards.Peek().sprite;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadOrDefaultData();
        }

        private void LoadOrDefaultData()
        {
            ClearStack();
            var ordersSaveData = SaveManager.Instance.LoadOrDefault(
                new OrdersSaveData(),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");

            var rewardStackDict =
                JsonConvert.DeserializeObject<Dictionary<int, string>>(ordersSaveData.rewardsStackJSonFormat);

            for (var i = rewardStackDict.Values.Count - 1; i >= 0; i--)
            {
                var item = GameDataHelper.AllItems.Find(item => item.name == rewardStackDict[i]);

                if (item == null)
                {
                    Debug.LogError($"Не удалось загрузить награду стека [{rewardStackDict[i]}].");
                    continue;
                }

                AppendReward(item);
            }
        }

        private void Update()
        {
            var rewardsCount = Rewards.Count;
            gameObject.SetActive(rewardsCount > 0);
            rewardsCountText.text = rewardsCount.ToString();
        }

        private void OnEnable()
        {
            GameManager.Instance.LevelChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
            GameManager.Instance.LevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(int _)
        {
            LoadOrDefaultData();
        }
    }
}
