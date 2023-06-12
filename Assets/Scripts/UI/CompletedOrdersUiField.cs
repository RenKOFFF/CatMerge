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

            Initialize(OrderManager.GetOrdersNeededToCompleteLevelCount(GameManager.Instance.CurrentLevel), orderManager.CompletedOrdersCount);
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
            UpdateMaxValue(OrderManager.GetOrdersNeededToCompleteLevelCount(GameManager.Instance.CurrentLevel));
        }
    }
}
