using System;
using System.Collections.Generic;
using GameData;
using Newtonsoft.Json;

namespace SaveSystem
{
    [Serializable]
    public class GameplayData
    {
        public int CurrentEnergy { get; set; }
        public DateTime LastEnergyChangingTime { get; set; }
        public int Money { get; set; }
        public string OpenedLevelsDictionaryJSonFormat { get; set; }
        public string CompletedLevelsDictionaryJSonFormat { get; set; }
        public int CurrentLevel { get; set; }

        public GameplayData(GameManager gameManager)
        {
            CurrentEnergy = gameManager.Energy;
            LastEnergyChangingTime = gameManager.LastEnergyChangingTime;
            Money = gameManager.Money;

            var openedLevelsDictionary = gameManager.OpenedLevels;
            var completedLevelsDictionary = gameManager.CompletedLevels;
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(openedLevelsDictionary);
            CompletedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(completedLevelsDictionary);

            CurrentLevel = gameManager.CurrentLevel;
        }

        /// <summary>
        /// Default values
        /// </summary>
        public GameplayData()
        {
            CurrentEnergy = GameManager.EnergyController.MaxStartEnergy;
            LastEnergyChangingTime = DateTime.UtcNow;
            Money = 0;

            var startDict = new Dictionary<int, bool>();
            CompletedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(startDict);
            
            startDict.Add(1, true);
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(startDict);
            
            CurrentLevel = 0;
        }
    }
}
