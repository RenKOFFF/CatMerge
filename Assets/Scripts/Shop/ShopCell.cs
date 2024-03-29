﻿using System;
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
            {
                _button.onClick.AddListener(OnClickShopButton);
            }

            GameManager.Instance.MoneyChanged += OnMoneyCountChanged; 
            OnMoneyCountChanged(GameManager.Instance.Money);
        }

        private void OnDisable()
        {
            if (_button)
            {
                _button.onClick.RemoveListener(OnClickShopButton);
            }

            GameManager.Instance.MoneyChanged -= OnMoneyCountChanged; 
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

        private void OnMoneyCountChanged(int currentMoney)
        {
            SetInteractableByCurrentMoney(currentMoney);
        }

        private void SetInteractableByCurrentMoney(int currentMoney)
        {
            _button.interactable = currentMoney >= ShopData.Cost;
        }

        private void OnClickShopButton()
        {
            ShopController.Instance.ToBuy(this);
        }
    }

    internal enum CurrencyType
    {
        Money
    }
}