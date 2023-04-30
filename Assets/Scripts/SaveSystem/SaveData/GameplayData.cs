using System;
using System.Collections.Generic;
using GameData;
using Newtonsoft.Json;

namespace SaveSystem
{
    [Serializable]
    public class GameplayData
    {
        public int CurrentEnergy;
        public int Money;
        public string OpenedLevelsDictionaryJSonFormat;
        public int CurrentLevel;

        public GameplayData(GameManager gameManager)
        {
            CurrentEnergy = gameManager.Energy;
            Money = gameManager.Money;

            var openedLevelsDictionary = gameManager.OpenedLevels;
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(openedLevelsDictionary);

            CurrentLevel = gameManager.CurrentLevel;
        }

        /// <summary>
        /// Default values
        /// </summary>
        public GameplayData()
        {
            CurrentEnergy = GameManager.EnergyController.MaxStartEnergy;
            Money = 0;

            var startDict = new Dictionary<int, bool>();
            startDict.Add(1, true);
            OpenedLevelsDictionaryJSonFormat = JsonConvert.SerializeObject(startDict);

            CurrentLevel = 0;
        }
    }
}