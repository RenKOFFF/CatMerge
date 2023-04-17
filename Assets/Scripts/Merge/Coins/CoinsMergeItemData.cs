using GameData;
using UnityEngine;

namespace Merge.Coins
{
    [CreateAssetMenu(menuName = "Custom/CoinsMergeItem")]
    public class CoinsMergeItemData : MergeItemData
    {
        [Min(1), SerializeField] private int _coins = 1;

        public void GiveCoins()
        {
            GameManager.Instance.AddMoney(_coins);
        }
    }
}