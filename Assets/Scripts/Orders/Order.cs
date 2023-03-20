using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private OrderPart orderPartPrefab;
        [SerializeField] private Transform orderPartsParent;
        [SerializeField] private Button claimRewardButton;

        public void Initialize(OrderData orderData)
        {
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
            // claimRewardButton.enabled = ;
        }
    }
}
