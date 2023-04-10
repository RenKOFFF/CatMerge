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

        public Sprite sprite;

        [CanBeNull] public MergeItemData previousMergeItem;
        [CanBeNull] public MergeItemData nextMergeItem;

        [FormerlySerializedAs("SpawnProbability")] public AnimationCurve SpawnChance;
    }
}
