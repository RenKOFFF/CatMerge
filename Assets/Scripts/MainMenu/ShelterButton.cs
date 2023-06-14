using System;
using System.Linq;
using GameData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class ShelterButton : MonoBehaviour
    {
        [SerializeField] private int _shelterIndex = 1;
        [SerializeField] private TextMeshProUGUI _shelterIndexText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _button;

        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _shelterImage;

        [SerializeField] private ShelterButtonProgressBar _progressBar;
        [SerializeField] private LevelSwitcher _levelSwitcher;
        [SerializeField] private Canvas _sheltersCanvas;

        private void Start()
        {
            var shelterConfig = GameDataHelper.AllShelterData.Find(data => data.CurrentShelterIndex == _shelterIndex);

            if (shelterConfig)
            {
                _progressBar.SetShelter(_shelterIndex);
                _shelterImage.sprite = shelterConfig.ShelterSprite;
                _titleText.text = shelterConfig.ShelterName;
                _lockImage.gameObject.SetActive(false);
                _button.onClick.AddListener(ChangeShelter);
                _button.onClick.AddListener(CanvasOff);
            }
            else
            {
                _progressBar.gameObject.SetActive(false);
                _shelterImage.sprite = null;
                _shelterImage.color = Color.black;
            }

            _shelterIndexText.text = $"#{_shelterIndex}";
        }

        private void ChangeShelter()
        {
            _levelSwitcher.ChangeShelter(_shelterIndex);
        }

        private void CanvasOff()
        {
            _sheltersCanvas.gameObject.SetActive(false);
        }
    }
}