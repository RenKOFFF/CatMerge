using System.Collections.Generic;
using System.Linq;
using GameData;
using Orders;

namespace UI
{
    public class CompletedLevelUiField: CurrencyFillElement
    {
        private int _maxLevelInZone = 5;
        private int _openedLevels;

        private float _coeff;
        private void Start()
        {
            //TODO: это полное говнище, исправить, если проект будет не заброшен
            _openedLevels = GameManager.Instance.OpenedLevels.Values.Where(a => true).ToList().Count;

            _coeff = 100f / _maxLevelInZone;

            Initialize(_maxLevelInZone * _coeff, (_openedLevels - 1) * _coeff);
            _currencyText.text += "%";
            
            OrderManager.Instance.LevelCompleted += OnCompletedOrdersChanged;
        }

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= OnCompletedOrdersChanged;
        }

        private void OnCompletedOrdersChanged()
        {
            ChangeValue(_coeff * _openedLevels++);
            _currencyText.text += "%";
        }
    }
}