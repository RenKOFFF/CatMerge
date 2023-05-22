using System;
using System.Collections.Generic;
using System.Linq;
using Merge;
using Merge.Energy;
using Newtonsoft.Json;
using SaveSystem;
using UnityEngine;

namespace GameData
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        private int _money;

        public static GameManager Instance { get; private set; }
        public static EnergyController EnergyController;
        public bool IsGeneratorSpawned => GeneratorController.Instance.IsGeneratorSpawned;

        public int Money
        {
            get => _money;
            private set
            {
                _money = value;
                MoneyChanged?.Invoke(_money);
            }
        }

        public int Energy => EnergyController.CurrentEnergy;
        public DateTime LastEnergyChangingTime => EnergyController.LastEnergyChangingTime;
        public Dictionary<int, bool> OpenedLevels;

        public int CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }

        public event Action<int> LevelChanged;
        public event Action<int> MoneyChanged;

        private int _currentLevel;

        public void AddMoney(int amount)
        {
            Money += amount;
            SaveGameplayData();
        }
        
        public bool SpendMoney(int amount)
        {
            if (Money < amount) return false;
            
            Money -= amount;
            SaveGameplayData();
            return true;
        }

        public void AddEnergy(int amount)
        {
            EnergyController.Instance.AddEnergy(amount);
            SaveGameplayData();
        }

        public void SpendEnergy()
        {
            EnergyController.Instance.SpendEnergy();
            SaveGameplayData();
        }

        private void Awake()
        {
            Instance = this;
            EnergyController = GetComponent<EnergyController>();
        }

        private void Start()
        {
            var data = SaveManager.Instance.LoadOrDefault(new GameplayData());

            var lastEnergyChangingTime = data.LastEnergyChangingTime;
            var timePassedFromLastEnergyUpdate = DateTime.UtcNow - lastEnergyChangingTime;
            var offlineEnergyGainRaw = timePassedFromLastEnergyUpdate.TotalSeconds
                                       / EnergyController.TimeToRestoreOneEnergyInSeconds;
            var offlineEnergyGain = (int) Math.Floor(offlineEnergyGainRaw);

            Money = data.Money;
            EnergyController.LastEnergyChangingTime = lastEnergyChangingTime;
            EnergyController.SetEnergy(data.CurrentEnergy + offlineEnergyGain);
            OpenedLevels = JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
            CurrentLevel = data.CurrentLevel;
        }

        public void OpenNextLevelAndCloseCurrent()
        {
            if (OpenedLevels.ContainsKey(CurrentLevel))
                OpenedLevels[CurrentLevel] = false;

            OpenAllPossibleLevels();

            SaveGameplayData();
        }

        private void OpenAllPossibleLevels()
        {
            var levelData = GameDataHelper.AllLevelData;

            var currentLevelData = levelData.Where(i => i.CurrentLevelIndex == CurrentLevel).ToList();
            if (currentLevelData.Count > 1)
            {
                Debug.LogError("Level data more 1; Fix this");
                return;
            }

            var nextLevelIndexes = new List<int>();
            if (currentLevelData.Count == 0 || currentLevelData[0].NextLevelIndexes.Count == 0)
            {
                //TODO:this is debug code
                if (currentLevelData.Count != 0 && currentLevelData[0].NextLevelIndexes.Count == 0)
                    Debug.Log("There is level data, but next level list is empty");
                
                nextLevelIndexes.Add(CurrentLevel + 1);
            }
            else nextLevelIndexes = currentLevelData[0].NextLevelIndexes;


            foreach (var nextLevelIndex in nextLevelIndexes)
            {
                OpenedLevels.TryAdd(nextLevelIndex, true);
            }
        }

        private void SaveGameplayData()
        {
            SaveManager.Instance.Save(new GameplayData(Instance));
        }

        public void ChangeLevel(int lvlIndex)
        {
            if (CurrentLevel == lvlIndex) return;

            CurrentLevel = lvlIndex;
            SaveGameplayData();

            LevelChanged?.Invoke(_currentLevel);
        }

        private void OnApplicationQuit()
        {
            SaveGameplayData();
            MergeController.Instance.SaveMField();
        }
    }
}
