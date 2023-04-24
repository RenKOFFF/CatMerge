using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Merge;
using UnityEngine;

namespace Orders.Data
{
    public class OrderPartData
    {
        public MergeItemData NeededItem { get; }
        public int RewardMoney { get; }

        public OrderPartData(MergeItemData neededItem)
        {
            NeededItem = neededItem;
            RewardMoney = GetRandomRewardMoney();
        }

        private int GetRandomRewardMoney()
            => Random.Range(1, 4 + 1) * NeededItem.ComplexityLevel;
    }

    public class OrderData
    {
        public List<OrderPartData> Parts { get; } = new();

        [CanBeNull] public MergeItemData RewardItem { get; }
        public bool ContainsRewardItem => RewardItem != null;

        public int RewardMoney => Parts.Select(p => p.RewardMoney).Sum();
        public bool ContainsRewardMoney { get; }

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
