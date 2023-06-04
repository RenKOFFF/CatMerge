using GameData;
using UnityEngine;

namespace Merge.Coins
{
    [CreateAssetMenu(menuName = "Custom/CoinsMergeItem")]
    public class CoinsMergeItemData : MergeItemData, ICurrencyValueOwner
    {
        [Min(1), SerializeField] private int _coins = 1;
        public int Value => _coins;
        

        public void GiveCoins()
        {
            GameManager.Instance.AddMoney(_coins);
        }
    }
}