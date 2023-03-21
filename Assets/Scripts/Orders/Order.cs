using Merge;
using UnityEngine;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private GameObject claimRewardButton;

        private OrderData _orderData;

        public void Initialize(OrderData orderData)
        {
            _orderData = orderData;

            foreach (var orderPartData in orderData.Parts)
            {
                var orderPart = Instantiate(orderPartPrefab, orderPartsParent, false);
                orderPart.Initialize(orderPartData);
            }
        }

        public void ClaimReward()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            var areAllNeededItemsOnFieldNow = true;
            var mergeItemsOnField = MergeController.Instance.MergeItemDatas;

            foreach (var orderPartData in _orderData.Parts)
                areAllNeededItemsOnFieldNow &= mergeItemsOnField.Contains(orderPartData.NeededItem);

            claimRewardButton.SetActive(areAllNeededItemsOnFieldNow);
        }
    }
}
