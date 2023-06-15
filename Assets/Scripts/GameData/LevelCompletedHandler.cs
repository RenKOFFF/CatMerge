using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameData
{
    public class LevelCompletedHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _cat;
        [SerializeField] private GameObject _popup;
        [SerializeField] private Image _shade;
        [SerializeField] private Color _openedColor;

        private UnityAction OnOpenMenu;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tween;

        private void Start()
        {
            float durationPanel = 0.8f;
            _shade.DOColor(_openedColor, durationPanel);
            _popup.transform.localScale = Vector3.zero;
            _popup.transform
                .DOScale(Vector3.one, durationPanel);
            //.OnComplete(() => PlayCatAnimation());
            PlayCatAnimation();
        }

        private void PlayCatAnimation()
        {
            float durationCat = 1f;

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