using System;
using GameData;
using Merge;
using Orders;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shop
{
    [DefaultExecutionOrder(-1)]
    public class ShopController : MonoBehaviour
    {
        public static ShopController Instance;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }

        private void Start()
        {
            SetActiveShop(false);
        }

        public void SetActiveShop(bool value) => gameObject.SetActive(value);

        public bool ToBuy(ShopCell shopCell)
        {
            if (GameManager.Instance.Money < shopCell.ShopData.Cost)
                return false;

            var spawnIndex = Random.Range(0, shopCell.ShopData.Items.Length);

            RewardsStack.Instance.AppendReward(shopCell.ShopData.Items[spawnIndex]);
            GameManager.Instance.SpendMoney(shopCell.ShopData.Cost);

            return true;
        }
    }
}