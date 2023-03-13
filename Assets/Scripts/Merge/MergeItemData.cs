using JetBrains.Annotations;
using UnityEngine;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/MergeItem")]
    public class MergeItemData : ScriptableObject
    {
        public bool IsFinalItem => nextMergeItem == null;

        public Sprite sprite;

        [CanBeNull] public MergeItemData nextMergeItem;
    }
}
