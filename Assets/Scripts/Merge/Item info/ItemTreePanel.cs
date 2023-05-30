using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemTreePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemGroupName;
        [SerializeField] private Transform itemTreeNodesParent;
        [SerializeField] private ItemTreeNode itemTreeNodePrefab;
        [SerializeField] private Transform itemPointersParent;
        [SerializeField] private Image itemPointerPrefab;

        private const int ItemsInRow = 4;

        public void Initialize(MergeItem selectedItem)
        {
            itemTreeNodesParent.DestroyChildren();
            itemPointersParent.DestroyChildren();

            var currentDisplayItem = selectedItem.MergeItemData.FirstItem;

            var itemImagesCount = 0;
            var itemsGroupName = currentDisplayItem.GroupName;

            itemGroupName.text = !string.IsNullOrEmpty(itemsGroupName)
                ? itemsGroupName
                : "Загадочные предметы";

            do
            {
                SpawnNode(currentDisplayItem);
                currentDisplayItem = currentDisplayItem.nextMergeItem;
                itemImagesCount++;
            } while (currentDisplayItem != null);

            var rowsCount = Mathf.CeilToInt((float) itemImagesCount / ItemsInRow);
            var pointersCount = itemImagesCount - rowsCount;

            for (var i = 0; i < pointersCount; i++)
                Instantiate(itemPointerPrefab, itemPointersParent, false);

            Close();
        }

        private void SpawnNode(MergeItemData currentDisplayItem)
        {
            var node = Instantiate(itemTreeNodePrefab, itemTreeNodesParent, false);
            node.Initialize(currentDisplayItem);
        }

        private void Awake()
        {
            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
