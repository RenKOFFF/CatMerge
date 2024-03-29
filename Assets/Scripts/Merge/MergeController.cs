﻿using System.Collections.Generic;
using System.Linq;
using GameData;
using JetBrains.Annotations;
using Merge.Generator;
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

        public Dictionary<string, int> MergeItemGroupNameToUnlockedComplexityLevel { get; set; } = new();

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
            if (MergingItem == null || !MergingItem.IsMoving)
                return;

            if (MergingItem.TryMergeIn(droppedOnItem, SpawnRandomItemWithChance))
            {
                UpdateUnlockedComplexityLevel(droppedOnItem.MergeItemData);
                ItemInfo.Instance.Initialize(droppedOnItem);
                SaveMField();
                ResetMergingItem();
                return;
            }

            droppedOnItem.TrySwapData(MergingItem);
            ItemInfo.Instance.Initialize(droppedOnItem);
            SaveMField();
            ResetMergingItem();
        }

        public void OnClick(MergeItem clickedItem)
        {
            ItemInfo.Instance.Initialize(clickedItem);
            SaveMField();
        }

        private void SetMergingItem(MergeItem clickedItem)
        {
            if (clickedItem.IsEmpty)
                return;

            MergingItem = clickedItem;
            ItemInfo.Instance.Initialize(MergingItem);
        }

        private void ResetMergingItem()
        {
            MergingItem = null;
        }

        private void SpawnRandomItemWithChance(int itemLevel)
        {
            var itemSpawnChance = .1f * itemLevel;

            var spawnItemIndex = Random.Range(0, GameDataHelper.AllRewardItems.Count);
            var spawnItem = GameDataHelper.AllRewardItems[spawnItemIndex];

            var spawnChance = Random.Range(0f, 1f);
            if (spawnChance <= itemSpawnChance)
            {
                SpawnItem(spawnItem);
            }
        }

        public bool SpawnItem(MergeItemData spawnItem)
        {
            var spawnCellIndex = GetEmptyCellIndex();
            if (spawnCellIndex == -1)
            {
                Debug.Log("can't spawn");
                return false;
            }

            var isItemSpawned = mergeCells[spawnCellIndex].MergeItem.TrySetData(spawnItem, false);

            if (isItemSpawned)
            {
                SaveMField();
                UpdateUnlockedComplexityLevel(mergeCells[spawnCellIndex].MergeItem.MergeItemData);
            }

            return isItemSpawned;
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
            LoadUnlockedComplexityLevels();
            GeneratorController.Instance.SpawnGenerator();
        }

        private void LoadMergeFieldData()
        {
            if (GameManager.Instance.CurrentLevel == 0) return;

            var loadedData = SaveManager.Instance.LoadOrDefault(new LevelSaveData(Instance.MergeCells.Length),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");

            var dict = JsonConvert.DeserializeObject<Dictionary<int, string>>(loadedData
                .CellsDictionaryJSonFormat);

            for (int i = 0; i < mergeCells.Length; i++)
            {
                if (dict.TryGetValue(i, out var dataName))
                {
                    var mergeItemData = GameDataHelper.AllItems.Find(a => a.name == dataName);
                    MergeCells[i].MergeItem.TrySetData(mergeItemData, true);
                }
                else MergeCells[i].MergeItem.TrySetData(null, true);
            }

            GeneratorController.Instance.SetIsGeneratorSpawned(loadedData.IsGeneratorSpawned);
        }

        public void SaveMField()
        {
            if (GameManager.Instance.CurrentLevel == 0) return;

            SaveManager.Instance.Save(new LevelSaveData(Instance, GameManager.Instance.IsGeneratorSpawned),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");
        }

        public int GetUnlockedComplexityLevel(MergeItemData mergeItemData)
            => MergeItemGroupNameToUnlockedComplexityLevel.GetValueOrDefault(mergeItemData.GroupName);

        public void UpdateUnlockedComplexityLevel(MergeItemData mergeItemData)
        {
            var groupName = mergeItemData.GroupName;
            var complexityLevel = mergeItemData.ComplexityLevel;

            if (!MergeItemGroupNameToUnlockedComplexityLevel.ContainsKey(groupName)
                || MergeItemGroupNameToUnlockedComplexityLevel[groupName] < complexityLevel)
            {
                MergeItemGroupNameToUnlockedComplexityLevel[groupName] = complexityLevel;
                SaveUnlockedComplexityLevels();
            }
        }

        private void SaveUnlockedComplexityLevels()
        {
            SaveManager.Instance.Save(
                MergeItemGroupNameToUnlockedComplexityLevel,
                SaveFileNames.UnlockedMergeItemComplexityLevels);
        }

        private void LoadUnlockedComplexityLevels()
        {
            MergeItemGroupNameToUnlockedComplexityLevel = SaveManager.Instance.LoadOrDefault(
                MergeItemGroupNameToUnlockedComplexityLevel,
                SaveFileNames.UnlockedMergeItemComplexityLevels);
        }
    }
}