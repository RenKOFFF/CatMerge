using GameData;
using Orders;

namespace UI
{
    public class CompletedOrdersUiField : CurrencyFillElement
    {
        private void Start()
        {
            var orderManager = OrderManager.Instance;
            orderManager.CompletedOrdersChanged += OnCompletedOrdersChanged;
            GameManager.Instance.LevelChanged += OnLevelChanged;

            Initialize(OrderManager.GetOrdersNeededToCompleteCurrentLevelCount(), orderManager.CompletedOrdersCount, true);
        }

        private void OnDestroy()
        {
            OrderManager.Instance.CompletedOrdersChanged -= OnCompletedOrdersChanged;
            GameManager.Instance.LevelChanged -= OnLevelChanged;
        }

        private void OnCompletedOrdersChanged(int currentValue)
        {
            ChangeValue(currentValue);
        }

        private void OnLevelChanged(int currentValue)
        {
            UpdateMaxValue(OrderManager.GetOrdersNeededToCompleteCurrentLevelCount());
        }
    }
}
