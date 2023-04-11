using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Merge.Selling;
using UnityEngine;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private MergeCell[] _spawnCells;
        [SerializeField] private SellButton sellButton;

        [CanBeNull] private MergeItem MergingItem { get; set; }

        public MergeCell[] SpawnCells => _spawnCells;

        public static MergeController Instance { get; private set; }

        public void OnBeginDrag(MergeItem clickedItem)
        {
            InteractWithMergeItem(clickedItem);
        }

        public void OnDrag()
        {
            if (MergingItem == null) return;

            var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MergingItem.transform.position = worldPosition;
        }

        public void OnEndDrag(MergeItem clickedItem)
        {
            InteractWithMergeItem(clickedItem);
        }

        public void OnClick(MergeItem clickedItem)
        {
            ActivateSellButton(clickedItem);
        }

        public void ResetMergingItem()
        {
            MergingItem = null;
        }

        public MergeItem FindMergeItemWithData(MergeItemData mergeItemData)
            => SpawnCells
                .Select(c => c.MergeItem)
                .FirstOrDefault(i => i.MergeItemData == mergeItemData);

        public List<MergeItem> FindMergeItemsWithData(MergeItemData mergeItemData)
            => SpawnCells
                .Select(c => c.MergeItem)
                .Where(i => i.MergeItemData == mergeItemData)
                .ToList();

        private void InteractWithMergeItem(MergeItem clickedItem)
        {
            if (MergingItem == null)
            {
                SetMergingItem(clickedItem);
                return;
            }

            if (!MergingItem.TryMergeIn(clickedItem))
            {
                clickedItem.TrySwapData(MergingItem);
            }

            ResetMergingItem();
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
