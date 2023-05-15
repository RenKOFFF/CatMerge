using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Custom/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int _currentLvlIndex;
        [SerializeField] private List<int> _nextLvlIndexes;
        
        public int CurrentLevelIndex => _currentLvlIndex;
        public List<int> NextLevelIndexes => _nextLvlIndexes;
    }
}