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

        public int MaxLevel
        {
            get
            {
                var maxLevel = ComplexityLevel;
                var currentItem = this;

                while (currentItem.nextMergeItem != null)
                {
                    maxLevel++;
                    currentItem = currentItem.nextMergeItem;
                }

                return maxLevel;
            }
        }

        public MergeItemData FirstItem
        {
            get
            {
                var currentItem = this;

                while (currentItem.previousMergeItem != null)
                    currentItem = currentItem.previousMergeItem;

                return currentItem;
            }
        }

        public string GroupName => FirstItem.itemsGroupName;

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
        public int ShelterItemIndex = 1;

        [FormerlySerializedAs("SpawnProbability")]
        public AnimationCurve SpawnChance;
        
        public ParticleSystem ParticleByUse => _particleByUse;

        [SerializeField] private ParticleSystem _particleByUse;
        
        public Sprite ParticleSprite => _particleSprite;

        [SerializeField] private Sprite _particleSprite;
    }
}
