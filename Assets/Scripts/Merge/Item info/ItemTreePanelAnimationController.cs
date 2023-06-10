using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Merge.Item_info
{
    public class ItemTreePanelAnimationController : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private Image _tree;
        
        [SerializeField] private Color _openedColor;
        [SerializeField] private Color _closedColor;
        
        private bool _animationInProcess;

        private void Awake()
        {
            _tree.transform.localScale = Vector3.zero;
            _tree.gameObject.SetActive(false);
            _bg.color = _closedColor;
            _bg.gameObject.SetActive(false);
        }
        
        public void SetActiveTree(bool isOpening)
        {
            _tree.gameObject.SetActive(true);
            
            if (_animationInProcess) return;

            _animationInProcess = true;

            var seq = DOTween.Sequence();

            float durationPanel = isOpening ? 0.8f : 0.4f;
            seq.Append(_tree.transform
                .DOScale(isOpening ? Vector3.one : Vector3.zero, durationPanel)
                .SetEase(isOpening ? Ease.OutBack : Ease.InBack));

            _bg.gameObject.SetActive(true);
            seq.Insert(0, _bg.DOColor(isOpening ? _openedColor : _closedColor, durationPanel));

            seq.Play().OnComplete(() =>
            {
                _animationInProcess = false;
                if (!isOpening) _bg.gameObject.SetActive(false);
            });
        }
    }
}
