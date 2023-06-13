using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Custom/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int _currentLvlIndex;
        [SerializeField] private int _currentShelterIndex = 1;
        [SerializeField] private List<int> _nextLvlIndexes;
        [SerializeField] private List<LevelData> _parallelLvlData;
        [SerializeField] private Sprite _bgWhenThisLevelCompleted;
        [SerializeField] private Sprite _bgWhenThisLevelCompletedLast;
        
        public int CurrentLevelIndex => _currentLvlIndex;
        public int CurrentShelterIndex => _currentShelterIndex;
        public List<int> NextLevelIndexes => _nextLvlIndexes;
        public List<LevelData> ParallelLvlData => _parallelLvlData;
        public Sprite BgWhenThisLevelCompleted => _bgWhenThisLevelCompleted;
        public Sprite BgWhenThisLevelCompletedLast => _bgWhenThisLevelCompletedLast;
    }
}