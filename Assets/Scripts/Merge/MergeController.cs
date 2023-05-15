﻿using System.Collections.Generic;
using System.Linq;
using GameData;
using JetBrains.Annotations;
using Merge.Item_info;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private MergeCell[] mergeCells;
        [SerializeField] private ItemInfo itemInfoPanel;

        [CanBeNull] private MergeItem MergingItem { get; set; }

        public MergeCell[] MergeCells => mergeCells;

        public static MergeController Instance { get; private set; }

        public MergeItem FindMergeItemWithData(MergeItemData mergeItemData)
            => MergeCells
                .Select(c => c.MergeItem)
                .FirstOrDefault(i => i.MergeItemData == mergeItemData);

        public List<MergeItem> FindMergeItemsWithData(MergeItemData mergeItemData)
            => MergeCells
                .Select(c => c.MergeItem)
                .Where(i => i.MergeItemData == mergeItemData)
                .ToList();

        public static int GetEmptyCellIndex()
        {
            var mergeCellsLength = Instance.MergeCells.Length;
            var spawnCellsIndexesArray = new int[mergeCellsLength];

            for (int i = 0; i < mergeCellsLength; i++)
            {
                spawnCellsIndexesArray[i] = i;
            }

            var shakedSpawnCellsIndexesArray = ShakeArray<int>.Shake(spawnCellsIndexesArray);
            var shakedIndex = 0;

            for (int i = 0; i < shakedSpawnCellsIndexesArray.Length; i++)
            {
                shakedIndex = shakedSpawnCellsIndexesArray[i];

                var isEmpty = MergeController.Instance.MergeCells[shakedIndex].GetComponentInChildren<MergeItem>()
                    .IsEmpty;
                if (isEmpty) return shakedIndex;
            }

            Debug.Log("Not empty cells");
            return -1;
        }

        public void OnBeginDrag(MergeItem clickedItem)
        {
            SetMergingItem(clickedItem);
        }

        public void OnDrag()
        {
            if (MergingItem == null)
                return;

            var worldPosition = (Vector2)Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MergingItem.transform.position = worldPosition;
        }

        public void OnDrop(MergeItem droppedOnItem)
        {
            if (MergingItem == null || MergingItem.TryMergeIn(droppedOnItem, SpawnRandomItemWithChance))
            {
                SaveMField();
                return;
            }

            droppedOnItem.TrySwapData(MergingItem);
            OpenItemInfo(droppedOnItem);
            SaveMField();
        }

        public void OnClick(MergeItem clickedItem)
        {
            OpenItemInfo(clickedItem);
            SaveMField();
        }

        private void SetMergingItem(MergeItem clickedItem)
        {
            if (clickedItem.IsEmpty)
                return;

            MergingItem = clickedItem;
            OpenItemInfo(MergingItem);
        }

        private void SpawnRandomItemWithChance(int itemLevel)
        {
            var itemSpawnChance = .1f * itemLevel;

            var spawnCellIndex = GetEmptyCellIndex();

            var spawnItemIndex = Random.Range(0, GameDataHelper.AllRewardItems.Count);
            var spawnItem = GameDataHelper.AllRewardItems[spawnItemIndex];

            var spawnChance = Random.Range(0f, 1f);
            if (spawnChance <= itemSpawnChance)
            {
                if (spawnCellIndex == -1)
                    RewardsStack.Instance.AppendReward(spawnItem);

                mergeCells[spawnCellIndex].MergeItem.TrySetData(spawnItem, false);
            }
        }

        private void OpenItemInfo(MergeItem selectedItem)
        {
            itemInfoPanel.Initialize(selectedItem);
        }
        {
            itemInfoPanel.Initialize(selectedItem);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (GameManager.Instance.CurrentLevel > 0)
                LoadLevel();
        }

        public void LoadLevel()
        {
            LoadMergeFieldData();
            GeneratorController.Instance.SpawnGenerator();
        }

        private void LoadMergeFieldData()
        {
            var loadedData = SaveManager.Instance.LoadOrDefault(new LevelSaveData(Instance.MergeCells.Length),
                GameManager.Instance.CurrentLevel.ToString());

            var dict = JsonConvert.DeserializeObject<Dictionary<int, string>>(loadedData
                .CellsDictionaryJSonFormat);

            for (int i = 0; i < mergeCells.Length; i++)
            {
                if (dict.TryGetValue(i, out var dataName))
                {
                    string directory = dataName.Split("_")[0];

                    var mergeItemData = Resources.Load<MergeItemData>($"MergeItems/{directory}/{dataName}");
                    MergeCells[i].MergeItem.TrySetData(mergeItemData, true);
                }
                else MergeCells[i].MergeItem.TrySetData(null, true);
            }

            GeneratorController.Instance.SetIsGeneratorSpawned(loadedData.IsGeneratorSpawned);
        }

        public void SaveMField()
        {
            SaveManager.Instance.Save(new LevelSaveData(Instance, GameManager.Instance.IsGeneratorSpawned),
                GameManager.Instance.CurrentLevel.ToString());
        }
    }
}
