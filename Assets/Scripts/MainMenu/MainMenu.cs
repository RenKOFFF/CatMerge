using System.Collections.Generic;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] _levelButtons;

    private void Start()
    {
        var data = SaveManager.Instance.LoadOrDefault(new GameplayData());
        var dict = JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].interactable = dict.TryGetValue(i, out var isOpened) && isOpened;
        }
    }
}