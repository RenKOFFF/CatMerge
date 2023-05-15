using System;
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

        private MergeItem _selectedItem;

        public void Initialize(MergeItem selectedItem)
        {
            _selectedItem = selectedItem;

            sellButton.Initialize(_selectedItem);
            itemImage.sprite = _selectedItem.MergeItemData.sprite;
            itemTitleText.text = $"{_selectedItem.MergeItemData.name}";
            itemLevelText.text = $"Уровень: {_selectedItem.MergeItemData.ComplexityLevel}";

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
