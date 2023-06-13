﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameData;
using Newtonsoft.Json;
using Orders;
using SaveSystem;
using UnityEditor;
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
        private bool _isLevelCompleted;

        private void Start()
        {
            _levelsData = GameDataHelper.AllLevelData;
            OrderManager.Instance.LevelCompleted += OnLevelCompleted;
            if (GameManager.Instance.CurrentLevel > 0)
            {
                var data = SaveManager.Instance.LoadOrDefault(new GameplayData());
                var openedLevels =
                    JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
                var completedLevels =
                    JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.CompletedLevelsDictionaryJSonFormat);

                completedLevels.TryGetValue(GameManager.Instance.CurrentLevel, out var isCompleted);

                if (openedLevels.TryGetValue(GameManager.Instance.CurrentLevel, out var isOpened) && !isOpened
                    && isCompleted)
                    SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
                else SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel - 1);
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
            var activeButtons = mainMenuButtons.Where(b => b.gameObject.activeInHierarchy).ToList();

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
                if (isOn)
                {
                    MainMenu.Instance.BackButton.targetGraphic
                        .DOColor(Color.white, _transitionDuration / 3f)
                        .SetEase(_ease);
                }
                else MainMenu.Instance.BackButton.targetGraphic.color = transparentColor;


                foreach (var button in activeButtons)
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

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= OnLevelCompleted;
        }

        private void OnLevelCompleted()
        {
            _isLevelCompleted = true;
        }

        private void SwitchBackgroundByLevelIndex(int currentLevel)
        {
            var currentLevelData =
                _levelsData.FirstOrDefault(i => i.CurrentLevelIndex == currentLevel);

            if (currentLevelData)
            {
                _backgroundOld.sprite = _background.sprite;
                _background.sprite = currentLevelData.BgWhenThisLevelCompleted;
            }
            else
            {
                Debug.LogError("Oh, shit!!!");
                return;
            }

            var completedParallelLevels = currentLevelData.ParallelLvlData.Count;
            if (completedParallelLevels == 0) return;

            foreach (var parallelLvl in currentLevelData.ParallelLvlData)
            {
                var isLevelCompleted =
                    GameManager.Instance.CompletedLevels.Keys.Count(l => l == parallelLvl.CurrentLevelIndex);
                if (isLevelCompleted > 0)
                {
                    completedParallelLevels--;
                    if (completedParallelLevels <= 0)
                    {
                        _background.sprite = currentLevelData.BgWhenThisLevelCompletedLast;
                    }
                }
            }
        }
    }
}