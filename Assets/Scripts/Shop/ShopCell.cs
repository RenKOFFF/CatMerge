using System;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopCell : MonoBehaviour
    {
        [SerializeField] private ShopCellData _shopCellData;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _button;
        
        public ShopCellData ShopData => _shopCellData;

        private void OnEnable()
        {
            if (_button)
                _button.onClick.AddListener(() => ShopController.Instance.ToBuy(this));
        }

        private void OnDisable()
        {
            if (_button)
                _button.onClick.RemoveListener(() => ShopController.Instance.ToBuy(this));
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _icon.sprite = _shopCellData.Icon;
            _costText.text = _shopCellData.Cost.ToString();
        }
    }

    internal enum CurrencyType
    {
        Money
    }
}