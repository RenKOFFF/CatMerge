using Merge;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "ShopCellData", menuName = "Custom/ShopCellData", order = 0)]
    public class ShopCellData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _cost;
        [SerializeField] private MergeItemData[] _items;

        public Sprite Icon => _icon;
        public int Cost => _cost;
        public MergeItemData[] Items => _items;
    }
}