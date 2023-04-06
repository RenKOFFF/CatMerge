using System.Collections.Generic;
using Merge;

namespace Orders
{
    public class OrderPartData
    {
        public MergeItemData NeededItem { get; }

        public OrderPartData(MergeItemData neededItem)
        {
            NeededItem = neededItem;
        }
    }

    public class OrderData
    {
        public List<OrderPartData> Parts { get; } = new();

        public void AddPart(OrderPartData orderPartData)
        {
            Parts.Add(orderPartData);
        }
    }
}
