using System.Collections.Generic;
using System.Linq;
using GameData;
using Orders;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CompletedLevelUiField : CurrencyFillElement
    {
        [SerializeField] TextMeshProUGUI _shelterIndexText;

        private int _maxLevelInZone = 5;
        private int _completedLevels;

        private float _coeff;

        private void Start()
        {
            //TODO: это полное говнище, исправить, если проект будет не заброшен
            _completedLevels = GameManager.Instance.CompletedLevels.Values.Where(a => true).ToList().Count;

            _coeff = 100f / _maxLevelInZone;

            Initialize(_maxLevelInZone * _coeff, (_completedLevels) * _coeff);
            _currencyText.text += "%";

            OrderManager.Instance.LevelCompleted += OnCompletedOrdersChanged;
            GameManager.Instance.ShelterChanged += OnShelterChanged;
            OnCompletedOrdersChanged();
        }

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= OnCompletedOrdersChanged;
            GameManager.Instance.ShelterChanged -= OnShelterChanged;
        }

        private void OnCompletedOrdersChanged()
        {
            ChangeValue(_coeff * GameManager.Instance.CompletedLevels.Values.Count);
            _currencyText.text += "%";
        }

        private void OnShelterChanged(int shelterIndex)
        {
            if (_shelterIndexText)
                _shelterIndexText.text = $"{shelterIndex}";
        }
    }
}