using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Orders;

namespace SaveSystem.SaveData
{
    [Serializable]
    public class OrdersSaveData
    {
        public string rewardDictJSonFormat, hasMoneyDictJSonFormat, partsDictJSonFormat, rewardsStackJSonFormat;
        public int CompletedOrdersCount;

        public OrdersSaveData(OrderManager orderManager)
        {
            List<Order> orderManagerActiveOrders = orderManager.ActiveOrders;
            
            Dictionary<int, string> rewardDict = new();
            Dictionary<int, bool> hasMoneyDict = new();
            Dictionary<int, Dictionary<int, string>> partsDict = new();
            
            CompletedOrdersCount = orderManager.CompletedOrdersCount;
            
            var rewardsStackArray = RewardsStack.Instance.Rewards.ToArray();
            var rewardsStackDict = new Dictionary<int, string>();
            
            for (int i = 0; i < rewardsStackArray.Length; i++)
            {
                rewardsStackDict.Add(i, rewardsStackArray[i].name);
            }
            
            for (int i = 0; i < orderManagerActiveOrders.Count; i++)
            {
                var orderDataRewardItem = orderManagerActiveOrders[i].OrderData.RewardItem;
                rewardDict.Add(i, orderDataRewardItem != null ? orderDataRewardItem.name : "");

                hasMoneyDict.Add(i, orderManagerActiveOrders[i].OrderData.ContainsRewardMoney);

                var partDict = new Dictionary<int, string>();
                for (int j = 0; j < orderManagerActiveOrders[i].OrderData.Parts.Count; j++)
                {
                    partDict.Add(j, orderManagerActiveOrders[i].OrderData.Parts[j].NeededItem.name);
                }

                partsDict.Add(i, partDict);
            }

            rewardDictJSonFormat = JsonConvert.SerializeObject(rewardDict);
            hasMoneyDictJSonFormat = JsonConvert.SerializeObject(hasMoneyDict);
            partsDictJSonFormat = JsonConvert.SerializeObject(partsDict);
            rewardsStackJSonFormat = JsonConvert.SerializeObject(rewardsStackDict);
        }

        public OrdersSaveData()
        {
            Dictionary<int, string> rewardDict = new();
            Dictionary<int, bool> hasMoneyDict = new();
            Dictionary<int, Dictionary<int, string>> partsDict = new();
            Dictionary<int, string> rewardsStack = new();

            rewardDictJSonFormat = JsonConvert.SerializeObject(rewardDict);
            hasMoneyDictJSonFormat = JsonConvert.SerializeObject(hasMoneyDict);
            partsDictJSonFormat = JsonConvert.SerializeObject(partsDict);
            rewardsStackJSonFormat = JsonConvert.SerializeObject(rewardsStack);
        }
    }
}