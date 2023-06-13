using System;
using System.Collections.Generic;
using GameData;
using Newtonsoft.Json;

namespace SaveSystem
{
    [Serializable]
    public class ShelterData
    {
        public string OpenedLevelsDictionaryJSonFormat { get; set; }
        public string CompletedLevelsDictionaryJSonFormat { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentShelter { get; set; }

        public ShelterData(GameManager gameManager)
        {
            var openedLevelsDictionary = gameManager.OpenedLevels;
            var completedLevelsDictionary = gameManager.CompletedLevels;
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(openedLevelsDictionary);
            CompletedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(completedLevelsDictionary);

            CurrentLevel = gameManager.CurrentLevel;
            CurrentShelter = gameManager.CurrentShelter;
        }

        /// <summary>
        /// Default values
        /// </summary>
        public ShelterData()
        {
            var startDict = new Dictionary<int, bool>();
            CompletedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(startDict);
            
            startDict.Add(1, true);
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(startDict);
            
            CurrentLevel = 0;
            CurrentShelter = 1;
        }
    }
}
