using System.Collections.Generic;
using Merge;

namespace Orders
{
    public class OrderPart
    {
        public MergeItemData NeededItem { get; }
        public int Count { get; } //TODO: подумать, а надо ли

        public OrderPart(MergeItemData neededItem, int count)
        {
            NeededItem = neededItem;
            Count = count;
        }
    }

    public class Order
    {
        public List<OrderPart> Parts { get; } = new();

        public void AddPart(OrderPart orderPart)
        {
            Parts.Add(orderPart);
        }
    }
}
