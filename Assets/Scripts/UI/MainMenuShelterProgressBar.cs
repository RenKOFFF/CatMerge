using System.Collections.Generic;
using System.Linq;
using GameData;
using Orders;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuShelterProgressBar : CurrencyFillElement
    {
        [SerializeField] TextMeshProUGUI _shelterIndexText;

        private int _maxLevelInShelter = 5;
        private int _completedLevels;

        private float _coeff;

        private void Start()
        {
            //TODO: это полное говнище, исправить, если проект будет не заброшен
            _completedLevels = GameManager.Instance.CompletedLevels.Values.Where(a => true).ToList().Count;

            _maxLevelInShelter = GetMaxLevelsOnShelter();
            _coeff = 100f / _maxLevelInShelter;

            Initialize(_maxLevelInShelter * _coeff, (_completedLevels) * _coeff);
            _currencyText.text += "%";

            OrderManager.Instance.LevelCompleted += ChangeValueWithPercentages;
            GameManager.Instance.ShelterChanged += OnShelterChanged;
            OnShelterChanged(GameManager.Instance.CurrentShelter);
        }

        private int GetMaxLevelsOnShelter()
        {
            return GameDataHelper.AllShelterData
                .First(s => s.CurrentShelterIndex == GameManager.Instance.CurrentShelter)
                .MaxLevelsInTheShelter;
        }

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= ChangeValueWithPercentages;
            GameManager.Instance.ShelterChanged -= OnShelterChanged;
        }

        private void ChangeValueWithPercentages()
        {
            ChangeValue(_coeff * GameManager.Instance.CompletedLevels.Values.Count);
            _currencyText.text += "%";
        }

        private void OnShelterChanged(int shelterIndex)
        {
            _maxLevelInShelter = GetMaxLevelsOnShelter();
            _coeff = 100f / _maxLevelInShelter;
            
            UpdateMaxValue(_maxLevelInShelter * _coeff);
            
            if (_shelterIndexText)
                _shelterIndexText.text = $"{shelterIndex}";

            ChangeValueWithPercentages();
        }
    }
}