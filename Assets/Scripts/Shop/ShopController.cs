using System;
using GameData;
using Merge;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        public static ShopController Instance;

        private void Start()
        {
            if (!Instance) Instance = this;
        }

        public bool ToBuy(ShopCell shopCell)
        {
            if (GameManager.Instance.Money <= shopCell.ShopData.Cost)
                return false;

            var spawnIndex = Random.Range(0, shopCell.ShopData.Items.Length);
            
            var isSpawned = MergeController.Instance.SpawnItem(shopCell.ShopData.Items[spawnIndex]);
            if (isSpawned)
                GameManager.Instance.SpendMoney(shopCell.ShopData.Cost);
            
            return true;
        }
    }
}