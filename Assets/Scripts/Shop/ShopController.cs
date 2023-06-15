using System;
using System.Linq;
using DG.Tweening;
using GameData;
using Merge;
using Orders;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Shop
{
    [DefaultExecutionOrder(-1)]
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _shopButtons;

        [SerializeField] private GameObject _panel;
        [SerializeField] private Image _bg;

        [SerializeField] private Color _openedColor;
        [SerializeField] private Color _closedColor;

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
            _panel.transform.localScale = Vector3.zero;
            _bg.color = _closedColor;
            _bg.gameObject.SetActive(false);
        }

        public void SetActiveShop(bool isOpening)
        {
            if (_animationInProcess) return;

            _animationInProcess = true;
            var seq = DOTween.Sequence();

            float durationPanel = isOpening ? 0.8f : 0.4f;
            seq.Append(_panel.transform
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

            _bg.gameObject.SetActive(true);
            seq.Insert(0, _bg.DOColor(isOpening ? _openedColor : _closedColor, durationPanel));

            seq.Play().OnComplete(() =>
            {
                _animationInProcess = false;
                if (!isOpening) _bg.gameObject.SetActive(false);
            });
        }

        public bool ToBuy(ShopCell shopCell)
        {
            if (GameManager.Instance.Money < shopCell.ShopData.Cost)
                return false;

            var shopCellShopDataInCurrentShelter =
                shopCell.ShopData.Items
                    .Where(i =>
                    {
                        var unlockedComplexityLevel = MergeController.Instance.GetUnlockedComplexityLevel(i);
                        var maxItemLevel = Math.Min(unlockedComplexityLevel, 7);
                        var complexityLevel = i.ComplexityLevel;

                        return complexityLevel <= maxItemLevel &&
                               (i.ShelterItemIndex == GameManager.Instance.CurrentShelter ||
                                i.ShelterItemIndex == 0);
                    })
                    .ToList();

            if (shopCellShopDataInCurrentShelter.Count == 0)
                shopCellShopDataInCurrentShelter.Add(shopCell.ShopData.Items[0]);
            
            // if (shopCellShopDataInCurrentShelter.Length == 0)
            // {
            //     Debug.LogError("Oh shit!!! nechego to buy!!!!");
            //     return false;
            // }

            var spawnIndex = Random.Range(0, shopCellShopDataInCurrentShelter.Count);

            RewardsStack.Instance.AppendReward(shopCellShopDataInCurrentShelter[spawnIndex]);
            GameManager.Instance.SpendMoney(shopCell.ShopData.Cost);

            return true;
        }
    }
}