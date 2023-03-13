﻿using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Merge
{
    public class MergeController : MonoBehaviour
    {
        [CanBeNull] private MergeItem MergingItem { get; set; }
        private Vector3 StartItemPosition { get; set; }

        public static MergeController Instance { get; private set; }

        public void OnClick(MergeItem clickedItem, PointerEventData.InputButton inputButton)
        {
            switch (inputButton)
            {
                case PointerEventData.InputButton.Left:
                    InteractWithMergeItem(clickedItem);
                    break;
                default:
                    return;
            }
        }

        private void InteractWithMergeItem(MergeItem clickedItem)
        {
            if (MergingItem == null)
            {
                SetMergingItem(clickedItem);
                return;
            }

            if (!MergingItem.TryMergeIn(clickedItem))
                ResetMergingItem();
        }

        private void SetMergingItem(MergeItem clickedItem)
        {
            MergingItem = clickedItem;
            StartItemPosition = MergingItem.transform.position;
            MergingItem.GetComponent<BoxCollider2D>().enabled = false;
        }

        public void ResetMergingItem()
        {
            if (MergingItem == null)
                return;

            MergingItem.transform.position = StartItemPosition;
            MergingItem.GetComponent<BoxCollider2D>().enabled = true;
            MergingItem = null;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (MergingItem == null) return;

            var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MergingItem.transform.position = worldPosition;
        }
    }
}
