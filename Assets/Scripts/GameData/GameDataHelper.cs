﻿using System.Collections.Generic;
using System.Linq;
using Merge;
using Merge.Coins;
using Merge.Generator;
using SaveSystem.SaveData;
using UnityEngine;

namespace GameData
{
    public static class GameDataHelper
    {
        public static List<MergeItemData> AllItems { get; }
        public static List<MergeItemData> AllMergeItems { get; }
        public static List<MergeItemData> AllRewardItems { get; }
        public static List<LevelData> AllLevelData { get; }
        public static List<ShelterConfig> AllShelterData { get;}
        
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
            
            AllLevelData = Resources
                .LoadAll<LevelData>("LevelData")
                .ToList();
            
            AllShelterData = Resources
                .LoadAll<ShelterConfig>("ShelterData")
                .ToList();
        }
    }
}
