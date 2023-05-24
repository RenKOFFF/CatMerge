using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/MergeItem")]
    public class MergeItemData : ScriptableObject
    {
        public bool IsFinalItem => nextMergeItem == null;
        public int SellPrice => ComplexityLevel;

        public int ComplexityLevel
        {
            get
            {
                var complexityLevel = 0;
                var currentItem = this;

                while (currentItem != null)
                {
                    complexityLevel++;
                    currentItem = currentItem.previousMergeItem;
                }

                return complexityLevel;
            }
        }

        [CanBeNull]
        public string itemName;

        public Sprite sprite;

        [CanBeNull]
        public MergeItemData previousMergeItem;

        [CanBeNull]
        public MergeItemData nextMergeItem;

        /// <summary>
        /// Название группы предметов. Указывать только у первого предмета цепочки.
        /// </summary>
        [CanBeNull]
        public string itemsGroupName;

        [FormerlySerializedAs("SpawnProbability")]
        public AnimationCurve SpawnChance;
    }
}
