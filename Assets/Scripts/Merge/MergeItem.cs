using System;
using System.Collections.Generic;
using DG.Tweening;
using GameData;
using Merge.Coins;
using Merge.Generator;
using Merge.Item_info;
using UI.Energy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Merge
{
    public class MergeItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private MergeItemData _mergeItemData;

        private int _clickCount;
        private float _timeBtwClick = .5f;

        private Image SpriteRenderer { get; set; }
        private CanvasGroup CanvasGroup { get; set; }

        public bool IsUsedForOrder { get; private set; }
        public bool IsGenerator { get; private set; }

        public bool IsEmpty => MergeItemData == null;
        public bool IsMoving => transform.localPosition != Vector3.zero;

        private static event Action<MergeItem> OnClick;

        public MergeItemData MergeItemData
        {
            get => _mergeItemData;
            private set
            {
                IsGenerator = value is GeneratorMergeItemData;

                SetUsedForOrder(false);
                _mergeItemData = value;
            }
        }

        public bool TryMergeIn(MergeItem itemToMergeIn, Action<int> callback = null)
        {
            if (IsEmpty
                || gameObject == itemToMergeIn.gameObject
                || MergeItemData.IsFinalItem
                || !Equals(itemToMergeIn))
            {
                return false;
            }

            itemToMergeIn.MergeItemData = MergeItemData.nextMergeItem;
            itemToMergeIn.RefreshSprite();

            DOTween.Sequence()
                .Append(itemToMergeIn.transform.DOScale(Vector3.zero, .3f))
                .Append(itemToMergeIn.transform.DOScale(Vector3.one, .3f))
                .SetEase(Ease.OutBounce);

            ClearItemCell();

            callback?.Invoke(itemToMergeIn.MergeItemData.ComplexityLevel);
            return true;
        }

        public void SetUsedForOrder(bool isUsed = true)
        {
            IsUsedForOrder = isUsed;
        }

        private bool Equals(MergeItem other)
            => MergeItemData == other.MergeItemData;

        private void RefreshSprite()
        {
            SpriteRenderer.sprite = !IsEmpty ? MergeItemData.sprite : null;
            SpriteRenderer.enabled = !IsEmpty;
        }

        private void Awake()
        {
            SpriteRenderer = GetComponent<Image>();
            CanvasGroup = GetComponent<CanvasGroup>();
            RefreshSprite();

            OnClick += ResetClickCountIfOtherItemIsClicked;
        }

        public bool TrySetData(MergeItemData data, bool forceSet)
        {
            if (forceSet || MergeItemData is null)
            {
                MergeItemData = data;
                RefreshSprite();

                if (data != null)
                {
                    var sequence = DOTween.Sequence()
                        .Append(transform.DOScale(new Vector3(.9f, .9f), .2f))
                        .Append(transform.DOScale(new Vector3(1.2f, 1.2f), .2f))
                        .Append(transform.DOScale(new Vector3(1, 1), .3f))
                        .SetEase(Ease.OutBounce);

                    //sequence.Append(transform.DOShakeScale(1, .3f));
                }

                return true;
            }

            return false;
        }

        public bool TrySwapData(MergeItem fromMergeItem)
        {
            if (Equals(fromMergeItem))
                return false;

            var tempMergeItemData = MergeItemData;

            TrySetData(fromMergeItem.MergeItemData, true);
            fromMergeItem.TrySetData(tempMergeItemData, true);

            return true;
        }

        public void ClearItemCell()
        {
            TrySetData(null, true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
            var slotTransform = transform.parent.parent;
            slotTransform.SetAsLastSibling();

            MergeController.Instance.OnBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MergeController.Instance.OnDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetItem();
        }

        private void ResetItem()
        {
            CanvasGroup.blocksRaycasts = true;
            transform.DOLocalMove(Vector3.zero, .3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickCount++;

            OnClick?.Invoke(this);

            if (MergeItemData is GeneratorMergeItemData clickableData)
            {
                DOTween.Sequence()
                    .Append(transform.DOScale(new Vector3(.9f, .9f), .2f))
                    .Append(transform.DOScale(new Vector3(1.2f, 1.2f), .2f))
                    .Append(transform.DOScale(new Vector3(1, 1), .3f))
                    .SetEase(Ease.OutBounce);

                clickableData.Spawn();

                ItemInfo.Instance.Initialize(this);
                MergeController.Instance.SaveMField();
                return;
            }

            if (MergeItemData is EnergyMergeItemData doubleClickableData)
            {
                if (_clickCount >= 2)
                {
                    ResetClickCount();

                    var particles = SpawnParticle(doubleClickableData);
                    var target = FindObjectOfType<EnergyUiField>();

                    FlyToTargetAnimation(particles, target.gameObject, doubleClickableData.GiveEnergy);

                    DOTween.Sequence()
                        .Append(transform.DOScale(Vector3.one * 1.2f, .1f))
                        .Append(transform.DOScale(Vector3.zero, .1f))
                        .OnComplete(ClearItemCell);
                }

                ItemInfo.Instance.Initialize(this);
                MergeController.Instance.SaveMField();
                return;
            }

            if (MergeItemData is CoinsMergeItemData coinsMergeItemData)
            {
                if (_clickCount >= 2)
                {
                    ResetClickCount();

                    var particles = SpawnParticle(coinsMergeItemData);
                    var target = FindObjectOfType<ShowPlayerMoney>();

                    FlyToTargetAnimation(particles, target.gameObject, coinsMergeItemData.GiveCoins);

                    DOTween.Sequence()
                        .Append(transform.DOScale(Vector3.one * 1.2f, .1f))
                        .Append(transform.DOScale(Vector3.zero, .1f))
                        .OnComplete(ClearItemCell);
                }

                ItemInfo.Instance.Initialize(this);
                MergeController.Instance.SaveMField();
                return;
            }

            MergeController.Instance.OnClick(this);
        }

        private void ResetClickCountIfOtherItemIsClicked(MergeItem clickedItem)
        {
            if (clickedItem == this)
                return;

            ResetClickCount();
        }

        private void ResetClickCount()
        {
            _clickCount = 0;
        }

        private void FlyToTargetAnimation(List<ParticleSystem> particles, GameObject target, Action<int> callback)
        {
            var seq = DOTween.Sequence();
            var duration = 0f;

            for (int i = 0; i < particles.Count; i++)
            {
                if (i == 0) duration = particles[i].main.startLifetime.constant;

                var timePosition = i * particles[i].main.startLifetime.constantMax / particles.Count;

                var mainStartLifetime = particles[i].main;
                mainStartLifetime.startLifetime = particles[i].main.startLifetime.constant + timePosition;
                particles[i].Play();

                seq.Insert(timePosition,
                    particles[i].transform
                        .DOJump(target.transform.position,
                            Random.Range(-2f, -1f),
                            1,
                            duration)
                        .SetEase(Ease.InOutCirc)
                        .OnComplete(() => callback?.Invoke(1)));
            }

            seq.Play();
        }

        private List<ParticleSystem> SpawnParticle(ICurrencyValueOwner mergeItemData)
        {
            List<ParticleSystem> particles = new();
            for (int i = 0; i < mergeItemData.Value; i++)
            {
                var particle = Instantiate(MergeItemData.ParticleByUse, transform.parent, false);
                var particlesMain = particle.main;
                particlesMain.maxParticles = 1;

                var particlesTextureSheetAnimation = particle.textureSheetAnimation;
                particlesTextureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;

                for (int j = 0; j < particlesTextureSheetAnimation.spriteCount; j++)
                {
                    particlesTextureSheetAnimation.RemoveSprite(j);
                }

                particlesTextureSheetAnimation.AddSprite(MergeItemData.ParticleSprite);

                particles.Add(particle);
            }

            return particles;
        }

        public void Sell()
        {
            GameManager.Instance.AddMoney(MergeItemData.SellPrice);
            ClearItemCell();
        }

        private void OnDestroy()
        {
            OnClick -= ResetClickCountIfOtherItemIsClicked;
        }
    }
}
