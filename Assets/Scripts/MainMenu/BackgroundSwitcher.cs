using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameData;
using Orders;
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
        [SerializeField] private LevelData[] _levelsData;

        private bool _isLevelCompleted;

        private void Start()
        {
            OrderManager.Instance.LevelCompleted += OnLevelCompleted;
            if (GameManager.Instance.CurrentLevel > 1)
            {
                SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
            }
        }

        public void OnOpenMenu()
        {
            if (_isLevelCompleted)
            {
                PlayTransitionAnimation();
            }
        }

        [ContextMenu("PlayAnim")]
        private void PlayTransitionAnimation()
        {
            var levelButtons = MainMenu.Instance.LevelButtons;
            var backButton = MainMenu.Instance.BackButton;

            SwitchButtonsIntaractable(false);

            _backgroundOld.color = Color.white;
            Color transparentColor = _backgroundOld.color;
            transparentColor.a = 0;
            _backgroundOld.DOColor(transparentColor, _transitionDuration)
                .SetEase(_ease)
                .OnComplete(() => SwitchButtonsIntaractable(true));

            void SwitchButtonsIntaractable(bool value)
            {
                foreach (var levelButton in levelButtons)
                {
                    if (levelButton)
                        levelButton.interactable = value;
                }

                if (backButton)
                    backButton.interactable = value;
            }
        }

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= OnLevelCompleted;
        }

        private void OnLevelCompleted()
        {
            _isLevelCompleted = true;
            SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
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
                        _backgroundOld.sprite = _background.sprite;
                        _background.sprite = currentLevelData.BgWhenThisLevelCompletedLast;
                    }
                }
            }
        }
    }
}