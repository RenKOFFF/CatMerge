using System;
using System.Linq;
using GameData;
using Orders;

namespace UI
{
    public class ShelterButtonProgressBar : CurrencyFillElement
    {
        private int _maxLevelInShelter = 5;
        private int _completedLevels;

        private float _coeff;
        private int _shelterIndex;
        private bool _isInited;

        public void SetShelter(int shelterIndex)
        {
            _shelterIndex = shelterIndex;
            _completedLevels = GameManager.Instance.GetProgressInShelter(shelterIndex);

            _maxLevelInShelter = GetMaxLevelsOnShelter(shelterIndex);
            _coeff = 100f / _maxLevelInShelter;

            Initialize(_maxLevelInShelter * _coeff, (_completedLevels) * _coeff);
            _currencyText.text += "%";
            
            _isInited = true;
        }

        private void OnEnable()
        {
            if (!_isInited) return;

            ChangeValue(_coeff * GameManager.Instance.GetProgressInShelter(_shelterIndex));
            _currencyText.text += "%";
        }

        private int GetMaxLevelsOnShelter(int shelterIndex)
        {
            return GameDataHelper.AllShelterData
                .First(s => s.CurrentShelterIndex == shelterIndex)
                .MaxLevelsInTheShelter;
        }
    }
}