using Merge;
using Merge.Item_info;
using Orders.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class OrderPart : MonoBehaviour
    {
        [SerializeField] private Image orderPartImage;
        [SerializeField] private GameObject isItemOnFieldFlag;

        public bool IsItemOnField { get; private set; }

        private OrderPartData _orderPartData;
        private Image _background;

        public void Initialize(OrderPartData orderPartData)
        {
            _orderPartData = orderPartData;
            orderPartImage.sprite = _orderPartData.NeededItem.sprite;
            _background = GetComponent<Image>();
        }

        public void OpenItemInfo()
        {
            ItemInfo.Instance.Initialize(_orderPartData.NeededItem);
        }

        public void Complete()
        {
            MergeController.Instance.FindMergeItemWithData(_orderPartData.NeededItem).ClearItemCell();
            Destroy(gameObject);
            UseForOrderAllNeededItemsOnField(false);
        }

        private void UseForOrderAllNeededItemsOnField(bool isUsed = true)
        {
            var foundMergeItems = MergeController.Instance.FindMergeItemsWithData(_orderPartData.NeededItem);

            foreach (var item in foundMergeItems)
                item.SetUsedForOrder(isUsed);
        }

        private void Update()
        {
            var foundMergeItem = MergeController.Instance.FindMergeItemWithData(_orderPartData.NeededItem);

            IsItemOnField = foundMergeItem != null;
            isItemOnFieldFlag.SetActive(IsItemOnField);
            _background.color = IsItemOnField ? Order.CompletedOrderColor : new Color(1, 1, 1);

            if (IsItemOnField)
                UseForOrderAllNeededItemsOnField();
        }
    }
}
