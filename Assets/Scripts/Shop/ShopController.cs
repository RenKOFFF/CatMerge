using System;
using DG.Tweening;
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
        [SerializeField] private GameObject[] _shopButtons;
        private bool _animationInProcess;
        public static ShopController Instance;

        private void Awake()
        {
            if (!Instance) Instance = this;
            foreach (var button in _shopButtons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void SetActiveShop(bool isOpening)
        {
            if (_animationInProcess) return;

            _animationInProcess = true;
            var seq = DOTween.Sequence();

            float durationPanel = isOpening ? 0.8f : 0.4f;
            seq.Append(transform
                .DOScale(isOpening ? Vector3.one : Vector3.zero, isOpening ? 0.8f : 0.4f)
                .SetEase(isOpening ? Ease.OutBack : Ease.InBack));


            var buttonDuration = 0.3f;
            for (int i = 0; i < _shopButtons.Length; i++)
            {
                seq.Insert(durationPanel / 2f + i * buttonDuration / 2,
                    _shopButtons[i].transform
                        .DOScale(isOpening ? Vector3.one : Vector3.zero, buttonDuration)
                        .SetEase(isOpening ? Ease.OutBack : Ease.InBack));
            }


            seq.Play().OnComplete(() => _animationInProcess = false);
        }

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