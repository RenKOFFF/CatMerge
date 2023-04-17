using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Merge.Selling;
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
        }

        public void OnClick(MergeItem clickedItem)
        {
            ActivateSellButton(clickedItem);
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

        public void SaveMField()
        {
            //SaveManager.Instance.Save();
        }
    }
}