using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemInfo : MonoBehaviour
    {
        [SerializeField] private SellButton sellButton;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemTitleText;
        [SerializeField] private TMP_Text itemLevelText;
        [SerializeField] private ItemTreePanel itemTreePanel;

        private MergeItem _selectedItem;

        public void Initialize(MergeItem selectedItem)
        {
            if (selectedItem.IsEmpty)
                return;

            _selectedItem = selectedItem;

            sellButton.Initialize(_selectedItem);
            itemImage.sprite = _selectedItem.MergeItemData.sprite;
            itemLevelText.text = $"Уровень: {_selectedItem.MergeItemData.ComplexityLevel}";

            var itemName = _selectedItem.MergeItemData.itemName;
            itemTitleText.text = !string.IsNullOrEmpty(itemName)
                ? itemName
                : "Загадочный предмет";

            itemTreePanel.Initialize(_selectedItem);

            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_selectedItem.IsEmpty)
                Close();
        }
    }
}
