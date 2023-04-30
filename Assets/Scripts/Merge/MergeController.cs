﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using JetBrains.Annotations;
using Merge.Selling;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private MergeCell[] mergeCells;
        [SerializeField] private SellButton sellButton;

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
            if (MergingItem == null || MergingItem.TryMergeIn(droppedOnItem))
            {
                SaveMField();
                return;
            }

            droppedOnItem.TrySwapData(MergingItem);
            SaveMField();
        }

        public void OnClick(MergeItem clickedItem)
        {
            ActivateSellButton(clickedItem);
            SaveMField();
        }

        private void SetMergingItem(MergeItem clickedItem)
        {
            if (clickedItem.IsEmpty)
                return;

            MergingItem = clickedItem;
        }

        private void ActivateSellButton(MergeItem sellingItem)
        {
            sellButton.Initialize(sellingItem);
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

            GeneratorController.Instance.SetIsGeneratorSpawned(loadedData.IsGeneratorSpawned);

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
        }

        public void SaveMField()
        {
            SaveManager.Instance.Save(new LevelSaveData(Instance, GameManager.Instance.IsGeneratorSpawned),
                GameManager.Instance.CurrentLevel.ToString());
        }
    }
}