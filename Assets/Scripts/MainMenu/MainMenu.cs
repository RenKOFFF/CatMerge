using System.Collections.Generic;
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
        [SerializeField] private Button[] _levelButtons;
        [SerializeField] private Button _backButton;

        private Dictionary<int, bool> _openedLevels;

        private void Start()
        {
            GameManager.Instance.LevelChanged += OnLevelChanged;
            OrderManager.Instance.LevelCompleted += UpdateButtonInteractivity;
        
            UpdateButtonInteractivity();
        }

        private void OnDestroy()
        {
            GameManager.Instance.LevelChanged -= OnLevelChanged;
            OrderManager.Instance.LevelCompleted -= UpdateButtonInteractivity;
        }
    
        private void UpdateButtonInteractivity()
        {
            var data = SaveManager.Instance.LoadOrDefault(new GameplayData());
            var openedLevels = JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
            _openedLevels = openedLevels;
        
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                _levelButtons[i].gameObject.SetActive(openedLevels.TryGetValue(i + 1, out var isOpened) && isOpened);
            }

            OnLevelChanged(GameManager.Instance.CurrentLevel);
        }

        private void OnLevelChanged(int backLevelIndex)
        {
            _backButton.gameObject.SetActive(backLevelIndex > 0 && _openedLevels.TryGetValue(backLevelIndex, out var isOpened) && isOpened);
        }
    }
}