using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Newtonsoft.Json;
using Orders;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelButtonByShelter[] _levelButtons;
        [SerializeField] private Button _backButton;
        [SerializeField] private BackgroundSwitcher _backgroundSwitcher;

        private Dictionary<int, bool> _openedLevels;
        private Dictionary<int, bool> _completedLevels;

        public LevelButtonByShelter[] LevelButtons => _levelButtons;
        public Button BackButton => _backButton;
        public static MainMenu Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.LevelChanged += UpdateBackButtonInteractivity;
            GameManager.Instance.ShelterChanged += UpdateButtonInteractivity;
            OrderManager.Instance.LevelCompleted += UpdateButtonInteractivity;

            UpdateButtonInteractivity();
        }

        private void OnDestroy()
        {
            GameManager.Instance.LevelChanged -= UpdateBackButtonInteractivity;
            GameManager.Instance.ShelterChanged -= UpdateButtonInteractivity;
            OrderManager.Instance.LevelCompleted -= UpdateButtonInteractivity;
        }

        public void UpdateButtonInteractivity(int _)
        {
            UpdateButtonInteractivity();
        }

        public void UpdateButtonInteractivity()
        {
            var data = SaveManager.Instance.LoadOrDefault(new ShelterData(),
                $"Sh-{GameManager.Instance.CurrentShelter}");
            var openedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
            _openedLevels = openedLevels;

            var completedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.CompletedLevelsDictionaryJSonFormat);
            _completedLevels = completedLevels;

            var levelButtons = _levelButtons.SelectMany(b => b.LevelButtons);
            foreach (var levelButton in levelButtons)
            {
                levelButton.gameObject
                    .SetActive(openedLevels.TryGetValue(levelButton.LevelIndex, out var isOpened) && isOpened &&
                               levelButton.ShelterIndex == GameManager.Instance.CurrentShelter);
                if (levelButton.ShelterIndex == GameManager.Instance.CurrentShelter && isOpened &&
                    _completedLevels.TryGetValue(levelButton.LevelIndex, out var isCompleted) && isCompleted)
                {
                    levelButton.MakeHalo(_backgroundSwitcher.OnOpenMenu);
                }
            }

            UpdateBackButtonInteractivity(GameManager.Instance.CurrentLevel);
        }

        private void UpdateBackButtonInteractivity(int levelIndex)
        {
            _completedLevels.TryGetValue(levelIndex, out var isCompleted);

            _backButton.gameObject.SetActive(levelIndex > 0 &&
                                             _openedLevels.TryGetValue(levelIndex, out var isOpened) && isOpened &&
                                             !isCompleted);
        }

        public void ShowMenu()
        {
            gameObject.SetActive(true);
        }
    }

    [Serializable]
    public class LevelButtonByShelter
    {
        public int ShelterIndex;
        public LevelButton[] LevelButtons;
    }
}