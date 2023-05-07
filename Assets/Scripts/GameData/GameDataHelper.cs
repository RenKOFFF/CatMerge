using System.Collections.Generic;
using System.Linq;
using Merge;
using Merge.Coins;
using Merge.Generator;
using UnityEngine;

namespace GameData
{
    public static class GameDataHelper
    {
        public static List<MergeItemData> AllItems { get; }
        public static List<MergeItemData> AllMergeItems { get; }
        public static List<MergeItemData> AllRewardItems { get; }

        static GameDataHelper()
        {
            AllItems = Resources
                .LoadAll<MergeItemData>("MergeItems")
                .ToList();

            AllMergeItems = AllItems
                .Where(i => i.GetType() == typeof(MergeItemData))
                .ToList();

            AllRewardItems = AllItems
                .Where(i => i
                    is CoinsMergeItemData
                    or EnergyMergeItemData
                )
                .ToList();
        }
    }
}
