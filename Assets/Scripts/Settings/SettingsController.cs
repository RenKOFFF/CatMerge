using DG.Tweening;
using UnityEngine;

namespace Settings
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _buttons;
        private bool _animationInProcess;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        public void SetActiveSettings(bool isOpening)
        {
            if (_animationInProcess) return;

            _animationInProcess = true;

            var seq = DOTween.Sequence();

            float durationPanel = isOpening ? 0.8f : 0.4f;
            seq.Append(transform
                .DOScale(isOpening ? Vector3.one : Vector3.zero, durationPanel)
                .SetEase(isOpening ? Ease.OutBack : Ease.InBack));


            var buttonDuration = 0.3f;
            for (int i = 0; i < _buttons.Length; i++)
            {
                seq.Insert(durationPanel / 2f + i * buttonDuration / 2f,
                    _buttons[i].transform
                        .DOScale(isOpening ? Vector3.one : Vector3.zero, buttonDuration)
                        .SetEase(isOpening ? Ease.OutBack : Ease.InBack));
            }


            seq.Play().OnComplete(() => _animationInProcess = false);
        }
    }
}