using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class CurrencyFillElement : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _currencyText;
        [SerializeField] protected RectTransform _currencyFillRectTransform;
        [SerializeField] protected bool _showMaxValue;
        [SerializeField] private int _roundToDecimalCount = 2;

        private float _step;
        private float _maxValue;
        private float _currentValue;

        private bool _isInitialized;

        protected float MaxValue => _maxValue;

        protected void Initialize(float maxValue, float startValue = 0, bool showMaxValue = false)
        {
            if (_isInitialized) return;

            ChangeMaxValueVisibility(showMaxValue);

            UpdateMaxValue(maxValue);
            ChangeValue(startValue);
            _isInitialized = true;
        }

        protected void ChangeMaxValueVisibility(bool showMaxValue)
        {
            _showMaxValue = showMaxValue;
        }

        protected void UpdateMaxValue(float maxValue)
        {
            _maxValue = maxValue;
            _step = 1 / maxValue;
            UpdateText();
        }

        protected void ChangeValue(float newValue)
        {
            _currentValue = newValue;
            UpdateText();

            _currencyFillRectTransform.anchorMax =
                new Vector2(_maxValue == 0 ? 0 : Mathf.Clamp(_currentValue * _step, 0.00001f, 1),
                _currencyFillRectTransform.anchorMax.y);
        }

        protected void UpdateText()
        {
            _currencyText.text = $"{Math.Round(_currentValue, _roundToDecimalCount)}" + (_showMaxValue ? $"/{_maxValue}" : "");
        }
    }
}
