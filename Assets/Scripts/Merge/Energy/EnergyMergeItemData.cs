using Merge.Energy;
using UnityEngine;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/EnergyMergeItem")]
    public class EnergyMergeItemData : MergeItemData
    {
        [Min(1), SerializeField] private int _energy = 1;

        public void GetEnergy()
        {
            EnergyController.Instance.AddEnergy(_energy);
        }
    }
}