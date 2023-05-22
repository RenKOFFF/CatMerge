using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemTreePanel : MonoBehaviour
    {
        [SerializeField] private List<Image> itemsImages;

        public void Initialize(MergeItem selectedItem)
        {
            foreach (var image in itemsImages)
                image.sprite = null;

            var currentDisplayItem = selectedItem.MergeItemData;

            while (currentDisplayItem.previousMergeItem != null)
                currentDisplayItem = currentDisplayItem.previousMergeItem;

            var index = 0;

            while (currentDisplayItem.nextMergeItem != null)
            {
                itemsImages[index++].sprite = currentDisplayItem.sprite;
                currentDisplayItem = currentDisplayItem.nextMergeItem;
            }

            itemsImages[index].sprite = currentDisplayItem.sprite;

            Close();
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
