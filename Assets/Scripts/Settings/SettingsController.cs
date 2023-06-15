using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _buttons;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Image _bg;

        [SerializeField] private Color _openedColor;
        [SerializeField] private Color _closedColor;

        [SerializeField] private Image musicButton;
        [SerializeField] private Sprite musicIsOnSprite;
        [SerializeField] private Sprite musicIsOffSprite;

        [SerializeField] private Image soundsButton;
        [SerializeField] private Sprite soundsIsOnSprite;
        [SerializeField] private Sprite soundsIsOffSprite;

        private bool _animationInProcess;

        private static bool _isMusicOn = true;
        private static bool _isSoundsOn = true;

        private void Start()
        {
            _panel.transform.localScale = Vector3.zero;

            _bg.color = _closedColor;
            _bg.gameObject.SetActive(false);

            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        public void SetActiveSettings(bool isOpening)
        {
            if (_animationInProcess) return;

            _animationInProcess = true;
            UpdateMusicSprite();
            UpdateSoundsSprite();

            var seq = DOTween.Sequence();

            float durationPanel = isOpening ? 0.8f : 0.4f;
            seq.Append(_panel.transform
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

            _bg.gameObject.SetActive(true);
            seq.Insert(0, _bg.DOColor(isOpening ? _openedColor : _closedColor, durationPanel));

            seq.Play().OnComplete(() =>
            {
                _animationInProcess = false;
                if (!isOpening) _bg.gameObject.SetActive(false);
            });
        }

        public void SwitchMusic()
        {
            _isMusicOn = !_isMusicOn;
            UpdateMusicSprite();
        }

        private void UpdateMusicSprite()
        {
            musicButton.sprite = _isMusicOn ? musicIsOnSprite : musicIsOffSprite;
        }

        public void SwitchSounds()
        {
            _isSoundsOn = !_isSoundsOn;
            UpdateSoundsSprite();
        }

        private void UpdateSoundsSprite()
        {
            soundsButton.sprite = _isSoundsOn ? soundsIsOnSprite : soundsIsOffSprite;
        }
    }
}
