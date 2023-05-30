using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemTreeNode : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private Sprite lockedItemImage;

        public void Initialize(MergeItemData mergeItemData)
        {
            var complexityLevel = mergeItemData.ComplexityLevel;
            var isItemUnlocked = MergeController.Instance.GetUnlockedComplexityLevel(mergeItemData) >= complexityLevel;

            itemImage.sprite = isItemUnlocked ? mergeItemData.sprite : lockedItemImage;
            itemName.text = complexityLevel.ToString();
        }
    }
}
