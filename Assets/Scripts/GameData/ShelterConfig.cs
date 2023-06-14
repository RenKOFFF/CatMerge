using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace SaveSystem.SaveData
{
    [CreateAssetMenu(fileName = "Shelter_", menuName = "Custom/ShelterConfig", order = 0)]
    public class ShelterConfig : ScriptableObject
    {
        [SerializeField] private string _shelterName = "???";
        [SerializeField] private int _maxLevelsInTheShelter = 6;
        [SerializeField] private int _currentShelterIndex = 1;
        [SerializeField] private int _requiredCompletedLevelsCountToOpen;
        [SerializeField] private Sprite _shelterSprite;

        public string ShelterName => _shelterName;
        public int MaxLevelsInTheShelter => _maxLevelsInTheShelter;
        public int CurrentShelterIndex => _currentShelterIndex;
        public int RequiredCompletedLevelsCountToOpen => _requiredCompletedLevelsCountToOpen;
        public Sprite ShelterSprite => _shelterSprite;
    }
    
    [Serializable]
    public class LevelOnShelter
    {
        public int ShelterIndex;
        public int LevelIndex;
    }
}