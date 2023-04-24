using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Merge.Selling;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [FormerlySerializedAs("_spawnCells")] [SerializeField] private MergeCell[] mergeCells;
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
            var shookSpawnCellsArray = ShakeArray<MergeCell>.Shake(Instance.MergeCells);

            for (var i = 0; i < shookSpawnCellsArray.Length; i++)
            {
                if (Instance.MergeCells[i].GetComponentInChildren<MergeItem>().IsEmpty)
                    return i;
            }

            Debug.Log("Empty cells not found");
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

            var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MergingItem.transform.position = worldPosition;
        }

        public void OnDrop(MergeItem droppedOnItem)
        {
            if (MergingItem == null || MergingItem.TryMergeIn(droppedOnItem))
                return;

            droppedOnItem.TrySwapData(MergingItem);
        }

        public void OnClick(MergeItem clickedItem)
        {
            ActivateSellButton(clickedItem);
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
    }
}
