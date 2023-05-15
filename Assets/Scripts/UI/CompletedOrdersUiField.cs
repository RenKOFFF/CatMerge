using Orders;

namespace UI
{
    public class CompletedOrdersUiField : CurrencyFillElement
    {
        private void Start()
        {
            var orderManager = OrderManager.Instance;
            orderManager.ComoletedOrdersChanged += OnCompletedOrdersChanged;
            Initialize(orderManager.OrdersNeededToCompleteLevelCount, orderManager.CompletedOrdersCount);
        }

        private void OnDestroy()
        {
            OrderManager.Instance.ComoletedOrdersChanged -= OnCompletedOrdersChanged;
        }

        private void OnCompletedOrdersChanged(int currentValue)
        {
            ChangeValue(currentValue);
        }
    }
}