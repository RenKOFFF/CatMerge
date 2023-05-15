using Orders;

namespace UI
{
    public class CompletedOrdersUiField : CurrencyFillElement
    {
        private void Start()
        {
            var orderManager = OrderManager.Instance;
            orderManager.ComoletedOrdersChanged += OnCompletedOrdersChanged;
            Initialize(0, orderManager.CompletedOrdersCount);
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