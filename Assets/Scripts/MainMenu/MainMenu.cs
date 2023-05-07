using System;
using System.Collections.Generic;
using GameData;
using Merge;
using Newtonsoft.Json;
using Orders;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        GameManager.Instance.LevelChanged += OnLevelChanged;
        OrderManager.Instance.LevelComplited += UpdateButtonInteractivity;
        
        UpdateButtonInteractivity();
    }

    private void OnDestroy()
    {
        GameManager.Instance.LevelChanged -= OnLevelChanged;
        OrderManager.Instance.LevelComplited -= UpdateButtonInteractivity;
    }
    
    private void UpdateButtonInteractivity()
    {
        var data = SaveManager.Instance.LoadOrDefault(new GameplayData());
        var dict = JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
        
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].interactable = dict.TryGetValue(i + 1, out var isOpened) && isOpened;
        }
    }

    private void OnLevelChanged(int lvl)
    {
        _backButton.gameObject.SetActive(lvl > 0);
    }
}