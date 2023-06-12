using System;
using GameData;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class UiLevelButton : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _completedSprite;
        
        [SerializeField] private Image _image;
        
        [SerializeField] private MainMenuProgressPanel _progressPanel;
        [SerializeField] private GameObject _halo;


        public void ShowProgress()
        {
            _progressPanel.gameObject.SetActive(true);
            _progressPanel.ShowProgressByLevelIndex(GameManager.Instance.CurrentLevel);
            
            _image.sprite = _selectedSprite;
        }
    }
}