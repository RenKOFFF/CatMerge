using System;
using TMPro;
using UI;
using UnityEngine;

namespace GameData
{
    public class ShowPlayerMoney : CurrencyFillElement
    {
        private void Start()
        {
            GameManager.Instance.MoneyChanged += OnMoneyChanged;
            Initialize(0, GameManager.Instance.Money);
        }

        private void OnDestroy()
        {
            GameManager.Instance.MoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int money)
        {
            ChangeValue(money);
        }
    }
}
