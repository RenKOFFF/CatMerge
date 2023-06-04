using GameData;
using Merge.Coins;
using UnityEngine;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/EnergyMergeItem")]
    public class EnergyMergeItemData : MergeItemData, ICurrencyValueOwner
    {
        [Min(1), SerializeField] private int _energy = 1;
        public int Value => _energy;

        public void GiveEnergy(int value)
        {
            GameManager.Instance.AddEnergy(value);
        }
    }
}