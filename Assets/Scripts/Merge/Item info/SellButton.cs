using UnityEngine;

namespace Merge.Item_info
{
    public class SellButton : MonoBehaviour
    {
        private MergeItem _sellingItem;

        public void Initialize(MergeItem sellingItem)
        {
            _sellingItem = sellingItem;
        }

        public void Sell()
        {
            _sellingItem.Sell();
        }
    }
}
