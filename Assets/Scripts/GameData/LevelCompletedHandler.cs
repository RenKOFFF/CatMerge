using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GameData
{
    public class LevelCompletedHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _cat;

        private UnityAction OnOpenMenu;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tween;

        private void Start()
        {
            transform.localScale = Vector3.zero;

            float durationPanel = 0.8f;
            transform
                .DOScale(Vector3.one, durationPanel)
                .OnComplete(() => PlayCatAnimation());
        }

        private void PlayCatAnimation()
        {
            float durationCat = 2f;

            _tween = _cat.transform
                .DOLocalRotate(new Vector3(0, 0, 45), durationCat)
                .SetEase(Ease.InOutQuint)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void Initialize(UnityAction onOpenMenu)
        {
            OnOpenMenu = onOpenMenu;
        }

        public void OpenMenu()
        {
            _tween.Kill();
            OnOpenMenu();
        }
    }
}