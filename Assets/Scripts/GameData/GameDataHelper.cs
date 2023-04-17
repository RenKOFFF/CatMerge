using System.Collections.Generic;
using System.Linq;
using Merge;
using Merge.Generator;
using UnityEngine;

namespace GameData
{
    public static class GameDataHelper
    {
        public static List<MergeItemData> AllItems { get; }
        public static List<MergeItemData> AllMergeItems { get; }

        static GameDataHelper()
        {
            AllItems = Resources
                .LoadAll<MergeItemData>("MergeItems")
                .ToList();

            AllMergeItems = AllItems
                .Where(i => i
                    is not GeneratorMergeItemData
                    and not EnergyMergeItemData)
                .ToList();
        }
    }
}
