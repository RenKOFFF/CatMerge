using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using JetBrains.Annotations;
using Merge.Selling;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private MergeCell[] mergeCells;

        [SerializeField] private SellButton sellButton;

        [CanBeNull] private MergeItem MergingItem { get; set; }

        public MergeCell[] MergeCells => mergeCells;

        public static MergeController Instance { get; private set; }

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
                return;

            droppedOnItem.TrySwapData(MergingItem);

            SaveMField();
        }

        public void OnClick(MergeItem clickedItem)
        {
            ActivateSellButton(clickedItem);
            SaveMField();
        }

        public MergeItem FindMergeItemWithData(MergeItemData mergeItemData)
            => MergeCells
                .Select(c => c.MergeItem)
                .FirstOrDefault(i => i.MergeItemData == mergeItemData);

        public List<MergeItem> FindMergeItemsWithData(MergeItemData mergeItemData)
            => MergeCells
                .Select(c => c.MergeItem)
                .Where(i => i.MergeItemData == mergeItemData)
                .ToList();

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
            LoadMergeFieldData();
            GeneratorController.Instance.SpawnGenerator();
        }

        private void LoadMergeFieldData()
        {
            var loadedData = SaveManager.Instance.Load(new LevelSaveData(Instance, false));

            var dict = JsonConvert.DeserializeObject<Dictionary<int, string>>(loadedData
                .CellsDictionaryJSonFormat);

            GeneratorController.Instance.SetIsGeneratorSpawned(loadedData.IsGeneratorSpawned);
            
            for (int i = 0; i < mergeCells.Length; i++)
            {
                if (dict.TryGetValue(i, out var dataName))
                {
                    string directory = dataName.Split("_")[0];

                    var mergeItemData = Resources.Load<MergeItemData>($"MergeItems/{directory}/{dataName}");
                    MergeCells[i].MergeItem.TrySetData(mergeItemData, false);
                }
                else MergeCells[i].MergeItem.TrySetData(null, true);
            }
        }

        public void SaveMField()
        {
            SaveManager.Instance.Save(new LevelSaveData(Instance, GameManager.Instance.IsGeneratorSpawned));
        }
    }
}