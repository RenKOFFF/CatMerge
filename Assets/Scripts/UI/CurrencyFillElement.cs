using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class CurrencyFillElement : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _currencyText;
        [SerializeField] protected RectTransform _currencyFillRectTransform;

        protected float _step;
        protected float _maxValue;
        protected float _currentValue;
        
        protected bool _isInitialized;

        public void Initialize(float maxValue, float startValue = 0)
        {
            if (_isInitialized) return;

            _maxValue = maxValue;
            _step = 1 / maxValue;
            
            ChangeValue(startValue);
            _isInitialized = true;
        }

        public void ChangeValue(float newValue)
        {
            _currentValue = newValue;
            _currencyText.text = _currentValue.ToString(CultureInfo.InvariantCulture);
            
            _currencyFillRectTransform.anchorMax = 
                new Vector2(_maxValue == 0 ? 1 : Mathf.Clamp(_currentValue * _step, 0, 1),
                _currencyFillRectTransform.anchorMax.y);
        }
    }
}
