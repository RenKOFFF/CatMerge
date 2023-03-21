using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private List<MergeCell> mergeCells;

        [CanBeNull] private MergeItem MergingItem { get; set; }
        private Vector3 StartItemPosition { get; set; }

        public List<MergeCell> MergeCells => mergeCells;

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

        public void ResetMergingItem()
        {
            if (MergingItem == null)
                return;

            MergingItem.transform.position = StartItemPosition;
            //MergingItem.GetComponent<Image>().raycastTarget = true;
            MergingItem = null;
        }

        public MergeItem FindMergeItemWithData(MergeItemData mergeItemData)
            => mergeCells
                .Select(c => c.MergeItem)
                .FirstOrDefault(i => i.MergeItemData == mergeItemData);

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
            if (clickedItem.IsEmpty) return;

            MergingItem = clickedItem;
            StartItemPosition = MergingItem.transform.position;
            //MergingItem.GetComponent<Image>().raycastTarget = false;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
