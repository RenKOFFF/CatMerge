﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MergeItemData mergeItemData;

        private Image SpriteRenderer { get; set; }

        public bool TryMergeIn(MergeItem itemToMergeIn)
        {
            Debug.Log(gameObject == itemToMergeIn.gameObject);

            if (gameObject == itemToMergeIn.gameObject
                || mergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.mergeItemData = mergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();
            Destroy(gameObject);

            return true;
        }

        private bool Equals(MergeItem other)
            => mergeItemData == other.mergeItemData;

        private void RefreshSprite()
        {
            SpriteRenderer.sprite = mergeItemData.sprite;
        }

        private void Start()
        {
            SpriteRenderer = GetComponent<Image>();
            RefreshSprite();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MergeController.Instance.OnClick(this, eventData.button);
        }
    }
}
