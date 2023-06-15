using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class SellButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sellingCostText;
        [SerializeField] private Image _sellingCostIcon;
        [SerializeField] private TMP_Text itemDescriptionText;

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

            var shouldBeButtonActive = _sellingItem.MergeItemData.GetType() == typeof(MergeItemData);
            gameObject.SetActive(shouldBeButtonActive);

            var description = sellingItem.MergeItemData.GroupDescription;
            itemDescriptionText.text = description;
            itemDescriptionText.gameObject.SetActive(!shouldBeButtonActive && !string.IsNullOrWhiteSpace(description));
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
