using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class SellButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sellingCostText;
        [SerializeField] private Image _sellingCostIcon;

        private MergeItem _sellingItem;
        
        public void Initialize(MergeItem sellingItem)
        {
            _sellingItem = sellingItem;
            _sellingCostText.text = $"+{sellingItem.MergeItemData.SellPrice}";

            if (_sellingItem.IsEmpty)
            {
                Close();
                return;
            }

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
