using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Merge;

namespace Orders.Data
{
    public class OrderPartData
    {
        public MergeItemData NeededItem { get; set; }

        public OrderPartData(MergeItemData neededItem)
        {
            NeededItem = neededItem;
        }
    }

    public class OrderData
    {
        public List<OrderPartData> Parts { get; set; } = new();

        [CanBeNull] public MergeItemData RewardItem { get; set; }
        public bool ContainsRewardItem => RewardItem != null;

        public int RewardMoney => Parts.Sum(i => i.NeededItem.ComplexityLevel) * 10;
        public bool ContainsRewardMoney { get; set; }

        public OrderData([CanBeNull] MergeItemData rewardItem = null, bool containsRewardMoney = true)
        {
            RewardItem = rewardItem;
            ContainsRewardMoney = containsRewardMoney;
        }

        public void AddPart(OrderPartData orderPartData)
        {
            Parts.Add(orderPartData);
        }
    }
}
