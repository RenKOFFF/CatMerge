using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemTreeNode : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;

        public void Initialize(MergeItemData mergeItemData)
        {
            itemImage.sprite = mergeItemData.sprite;
            itemName.text = mergeItemData.ComplexityLevel.ToString();
        }
    }
}
