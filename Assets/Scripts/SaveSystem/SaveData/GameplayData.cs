using System;
using GameData;

namespace SaveSystem.SaveData
{
    [Serializable]
    public class GameplayData
    {
        public int CurrentEnergy { get; set; }
        public DateTime LastEnergyChangingTime { get; set; }
        public int Money { get; set; }
        
        public int CurrentLevel { get; set; }
        public int CurrentShelter { get; set; }

        public GameplayData(GameManager gameManager)
        {
            CurrentEnergy = gameManager.Energy;
            LastEnergyChangingTime = gameManager.LastEnergyChangingTime;
            Money = gameManager.Money;
            
            CurrentLevel = gameManager.CurrentLevel;
            CurrentShelter = gameManager.CurrentShelter;
        }

        public GameplayData()
        {
            CurrentEnergy = GameManager.EnergyController.MaxStartEnergy;
            LastEnergyChangingTime = DateTime.UtcNow;
            Money = 0;
            
            CurrentLevel = 0;
            CurrentShelter = 1;
        }
    }
}