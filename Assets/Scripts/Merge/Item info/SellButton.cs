using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class SellButton : MonoBehaviour
    {
        private MergeItem _sellingItem;

        public void Initialize(MergeItem sellingItem)
        {
            _sellingItem = sellingItem;
            GetComponent<Button>().interactable = _sellingItem.MergeItemData.GetType() == typeof(MergeItemData);
        }

        public void Sell()
        {
            _sellingItem.Sell();
        }
    }
}
