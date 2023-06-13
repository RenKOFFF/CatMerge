using System;
using GameData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private int _shelterIndex;

        [SerializeField] private LevelSwitcher _levelSwitcher;
        [SerializeField] private Canvas _mainMenuCanvas;
        [SerializeField] private Button _backToLevelButton;

        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _completedSprite;

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        [SerializeField] private MainMenuProgressPanel _progressPanel;
        [SerializeField] private GameObject _halo;

        public int LevelIndex => _levelIndex;
        public int ShelterIndex => _shelterIndex;
        public Button Button => _button;

        private void Start()
        {
            _button.onClick.AddListener(ShowProgress);
            _backToLevelButton.onClick.AddListener(ReturnToNormal);
        }

        private void ShowProgress()
        {
            _button.onClick.RemoveListener(ShowProgress);

            _progressPanel.gameObject.SetActive(true);
            _progressPanel.ShowProgressByLevelIndex(_shelterIndex, _levelIndex);

            _button.onClick.AddListener(LoadLevel);
            _button.onClick.AddListener(ReturnToNormal);

            _image.sprite = _selectedSprite;
        }

        private void LoadLevel()
        {
            _levelSwitcher.LoadLevel(_levelIndex);
            _mainMenuCanvas.gameObject.SetActive(false);
        }

        private void ReturnToNormal()
        {
            _button.onClick.RemoveAllListeners();
            _halo.SetActive(false);

            _button.onClick.AddListener(ShowProgress);
            _progressPanel.gameObject.SetActive(false);
            _image.sprite = _normalSprite;
        }

        public void MakeHalo(UnityAction callback)
        {
            _halo.SetActive(true);
            _image.sprite = _completedSprite;

            _button.onClick.AddListener(GameManager.Instance.OpenAllPossibleLevels);
            _button.onClick.AddListener(MainMenu.Instance.UpdateButtonInteractivity);
            _button.onClick.AddListener(callback);
            _button.onClick.AddListener(GameManager.Instance.CloseCurrentLevel);
            _button.onClick.AddListener(MainMenu.Instance.UpdateButtonInteractivity);
        }
    }
}