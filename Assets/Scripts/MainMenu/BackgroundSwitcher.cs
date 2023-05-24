using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Orders;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class BackgroundSwitcher : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private LevelData[] _levelsData;

        private void Start()
        {
            OrderManager.Instance.LevelCompleted += OnLevelCompleted;
            if (GameManager.Instance.CurrentLevel > 1)
            {
                SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
            }
        }

        private void OnDestroy()
        {
            OrderManager.Instance.LevelCompleted -= OnLevelCompleted;
        }

        private void OnLevelCompleted()
        {
            SwitchBackgroundByLevelIndex(GameManager.Instance.CurrentLevel);
        }

        private void SwitchBackgroundByLevelIndex(int currentLevel)
        {
            var currentLevelData =
                _levelsData.FirstOrDefault(i => i.CurrentLevelIndex == currentLevel);

            if (currentLevelData)
                _background.sprite = currentLevelData.BgWhenThisLevelCompleted;
            else
            {
                Debug.LogError("Oh, shit!!!");
                return;
            }

            var completedParallelLevels = currentLevelData.ParallelLvlData.Count;
            if (completedParallelLevels == 0) return;
            
            foreach (var parallelLvl in currentLevelData.ParallelLvlData)
            {
                var isLevelCompleted = GameManager.Instance.CompletedLevels.Keys.Count(l => l == parallelLvl.CurrentLevelIndex);
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