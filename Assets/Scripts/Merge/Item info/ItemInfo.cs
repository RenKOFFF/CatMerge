using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemInfo : MonoBehaviour
    {
        [SerializeField]
        private SellButton sellButton;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text itemTitleText;

        [SerializeField]
        private TMP_Text itemLevelText;

        [SerializeField]
        private ItemTreePanel itemTreePanel;

        private MergeItemData _selectedItemData;

        [CanBeNull]
        private MergeItem _selectedItem;

        public static ItemInfo Instance { get; private set; }

        public void Initialize(MergeItem selectedItem)
        {
            Initialize(selectedItem.MergeItemData);

            _selectedItem = selectedItem;
            sellButton.Initialize(_selectedItem);
        }

        public void Initialize(MergeItemData selectedItem)
        {
            if (selectedItem == null)
                return;

            _selectedItemData = selectedItem;

            itemImage.sprite = _selectedItemData.sprite;
            itemLevelText.text = $"Уровень: {_selectedItemData.ComplexityLevel}";

            var itemName = _selectedItemData.itemName;
            itemTitleText.text = !string.IsNullOrEmpty(itemName)
                ? itemName
                : "Загадочный предмет";

            sellButton.Close();
            _selectedItem = null;

            itemTreePanel.Initialize(_selectedItemData);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _selectedItem = null;
        }

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_selectedItem != null && _selectedItem.IsEmpty)
                Close();
        }
    }
}
