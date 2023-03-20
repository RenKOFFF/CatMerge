using System.Collections.Generic;
using Merge;

namespace Orders
{
    public class OrderPartData
    {
        public MergeItemData NeededItem { get; }
        public int Count { get; } //TODO: подумать, а надо ли

        public OrderPartData(MergeItemData neededItem, int count)
        {
            NeededItem = neededItem;
            Count = count;
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
