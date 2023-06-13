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
        [SerializeField] private LevelButton[] _levelButtons;
        [SerializeField] private Button _backButton;
        [SerializeField] private BackgroundSwitcher _backgroundSwitcher;

        private Dictionary<int, bool> _openedLevels;
        private Dictionary<int, bool> _completedLevels;
        
        public LevelButton[] LevelButtons => _levelButtons;
        public Button BackButton => _backButton;
        public static MainMenu Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.LevelChanged += UpdateBackButtonInteractivity;
            OrderManager.Instance.LevelCompleted += UpdateButtonInteractivity;

            UpdateButtonInteractivity();
        }

        private void OnDestroy()
        {
            GameManager.Instance.LevelChanged -= UpdateBackButtonInteractivity;
            OrderManager.Instance.LevelCompleted -= UpdateButtonInteractivity;
        }

        public void UpdateButtonInteractivity()
        {
            var data = SaveManager.Instance.LoadOrDefault(new ShelterData(), $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");
            var openedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
            _openedLevels = openedLevels;
            
            var completedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.CompletedLevelsDictionaryJSonFormat);
            _completedLevels = completedLevels;

            for (int i = 0; i < _levelButtons.Length; i++)
            {
                _levelButtons[i].gameObject.SetActive(openedLevels.TryGetValue(i + 1, out var isOpened) && isOpened);
                if (isOpened && _completedLevels.TryGetValue(i + 1, out var isCompleted) && isCompleted)
                {
                    _levelButtons[i].MakeHalo(_backgroundSwitcher.OnOpenMenu);
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
            
            // var levelButton = LevelButtons.First(a => a.LevelIndex == GameManager.Instance.CurrentLevel);
            // levelButton.MakeHalo(_backgroundSwitcher.OnOpenMenu);
        }
    }
}