using System.Collections.Generic;
using JetBrains.Annotations;
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
        [CanBeNull] public MergeItemData RewardItem { get; }

        public OrderData([CanBeNull] MergeItemData rewardItem = null)
        {
            RewardItem = rewardItem;
        }

        public void AddPart(OrderPartData orderPartData)
        {
            Parts.Add(orderPartData);
        }
    }
}
