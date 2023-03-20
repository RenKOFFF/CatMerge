using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class OrderPart : MonoBehaviour
    {
        [SerializeField] private Image orderPartImage;

        public void Initialize(OrderPartData orderPartData)
        {
            orderPartImage.sprite = orderPartData.NeededItem.sprite;
        }
    }
}
