using UnityEngine;

namespace Merge.Item_info
{
    public class SellButton : MonoBehaviour
    {
        private MergeItem _sellingItem;

        public void Initialize(MergeItem sellingItem)
        {
            _sellingItem = sellingItem;
            gameObject.SetActive(_sellingItem.MergeItemData.GetType() == typeof(MergeItemData));
        }

        public void Sell()
        {
            _sellingItem.Sell();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
