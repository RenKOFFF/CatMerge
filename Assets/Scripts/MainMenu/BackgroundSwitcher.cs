using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameData;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class BackgroundSwitcher : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _backgroundOld;
        [SerializeField] private float _transitionDuration = 1f;
        [SerializeField] private Ease _ease = Ease.Linear;

        private List<LevelData> _levelsData;

        private void OnEnable()
        {
            GameManager.Instance.ShelterChanged += UpdateBg;
        }

        private void OnDisable()
        {
            GameManager.Instance.ShelterChanged -= UpdateBg;
        }

        private void Start()
        {
            _levelsData = GameDataHelper.AllLevelData;
            UpdateBg(666);
        }

        private void UpdateBg(int _)
        {
            if (true) //GameManager.Instance.CurrentLevel > 0
            {
                var data = SaveManager.Instance.LoadOrDefault(new ShelterData(),
                    $"Sh-{GameManager.Instance.CurrentShelter}");
                var openedLevels =
                    JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
                var completedLevels =
                    JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.CompletedLevelsDictionaryJSonFormat);

                completedLevels.TryGetValue(GameManager.Instance.CurrentLevel, out var isCompleted);

                if (openedLevels.TryGetValue(GameManager.Instance.CurrentLevel, out var isOpened) && !isOpened
                    && isCompleted)
                    SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
                else SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel - 1, true);
            }
        }

        public void OnOpenMenu()
        {
            SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
            PlayTransitionAnimation();
        }

        [ContextMenu("PlayAnim")]
        private void PlayTransitionAnimation()
        {
            var mainMenuButtons = MainMenu.Instance.LevelButtons.ToList();
            var activeButtons = mainMenuButtons
                .Where(b => b.LevelButtons
                    .Any(levelButton => levelButton.gameObject.activeInHierarchy))
                .ToArray();


            Color transparentColor = Color.white;
            transparentColor.a = 0;
            _background.color = transparentColor;

            SwitchButtonsIntaractable(false);

            _background
                .DOColor(Color.white, _transitionDuration)
                .SetEase(_ease)
                .OnComplete(() => SwitchButtonsIntaractable(true));

            void SwitchButtonsIntaractable(bool isOn)
            {
                MainMenu.Instance.BackButton.interactable = isOn;
                MainMenu.Instance.ShelterButton.interactable = isOn;
                if (isOn)
                {
                    MainMenu.Instance.BackButton.targetGraphic
                        .DOColor(Color.white, _transitionDuration / 3f)
                        .SetEase(_ease);

                    MainMenu.Instance.ShelterButton.targetGraphic
                        .DOColor(Color.white, _transitionDuration / 3f)
                        .SetEase(_ease);
                }
                else
                {
                    MainMenu.Instance.BackButton.targetGraphic.color = transparentColor;
                    MainMenu.Instance.ShelterButton.targetGraphic.color = transparentColor;
                }


                var buttons = activeButtons.SelectMany(b => b.LevelButtons);
                foreach (var button in buttons)
                {
                    button.Button.interactable = isOn;
                    if (isOn)
                    {
                        button.Button.targetGraphic
                            .DOColor(Color.white, _transitionDuration / 3f)
                            .SetEase(_ease);
                    }
                    else button.Button.targetGraphic.color = transparentColor;
                }
            }
        }

        private void SwitchBackgroundByLevelIndex(int currentLevel, bool needForceLast = false)
        {
            if (currentLevel == -1) currentLevel = 0;

            var currentLevelData =
                _levelsData.FirstOrDefault(i =>
                    i.CurrentLevelIndex == currentLevel &&
                    i.CurrentShelterIndex == GameManager.Instance.CurrentShelter);

            if (currentLevelData)
            {
                _backgroundOld.sprite = _background.sprite;
                _background.sprite = needForceLast && currentLevelData.PreviousLevelData
                    ? currentLevelData.PreviousLevelData.BgWhenThisLevelCompleted
                    : currentLevelData.BgWhenThisLevelCompleted;
            }
            else
            {
                Debug.LogError("Oh, shit!!!");
                return;
            }

            var parallelLevels = currentLevelData.ParallelLvlData.Count;
            if (parallelLevels == 0 || needForceLast) return;

            foreach (var parallelLvl in currentLevelData.ParallelLvlData)
            {
                var isLevelCompleted =
                    GameManager.Instance.CompletedLevels.FirstOrDefault(l => l.Key == parallelLvl.CurrentLevelIndex).Value;
                
                var isLevelOpened =
                    GameManager.Instance.OpenedLevels.FirstOrDefault(l => l.Key == parallelLvl.CurrentLevelIndex).Value;
                
                if (isLevelCompleted && !isLevelOpened)
                {
                    parallelLevels--;
                    if (parallelLevels <= 0)
                    {
                        _background.sprite = currentLevelData.BgWhenThisLevelCompletedLast;
                    }
                }
            }
        }
    }
}