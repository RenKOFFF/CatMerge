using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/MergeItem")]
    public class MergeItemData : ScriptableObject
    {
        public bool IsFinalItem => nextMergeItem == null;

        public Sprite sprite;

        [CanBeNull] public MergeItemData nextMergeItem;
        
        [FormerlySerializedAs("SpawnProbability")] public AnimationCurve SpawnChance;
    }
}
