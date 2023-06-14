using System;
using System.Collections.Generic;
using System.Linq;
using Merge;
using Merge.Energy;
using Merge.Generator;
using Newtonsoft.Json;
using SaveSystem;
using SaveSystem.SaveData;
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
        public Dictionary<int, bool> CompletedLevels;

        public int CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }

        public int CurrentShelter
        {
            get => _currentShelter;
            private set => _currentShelter = value;
        }

        public event Action<int> LevelChanged;
        public event Action<int> ShelterChanged;
        public event Action<int> MoneyChanged;

        private int _currentLevel;
        private int _currentShelter = 1;

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

        public int GetProgressInShelter(int shelterIndex)
        {
            var shelterData = SaveManager.Instance.LoadOrDefault(
                new ShelterData(),
                $"Sh-{shelterIndex}");
            
            var completedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(shelterData.CompletedLevelsDictionaryJSonFormat);

            return completedLevels.Count;
        }
        
        private void Awake()
        {
            Instance = this;
            EnergyController = GetComponent<EnergyController>();
        }

        private void Start()
        {
            var gameplayData = SaveManager.Instance.LoadOrDefault(new GameplayData());

            CurrentLevel = gameplayData.CurrentLevel;
            CurrentShelter = gameplayData.CurrentShelter;

            var lastEnergyChangingTime = gameplayData.LastEnergyChangingTime;
            var timePassedFromLastEnergyUpdate = DateTime.UtcNow - lastEnergyChangingTime;
            var offlineEnergyGainRaw = timePassedFromLastEnergyUpdate.TotalSeconds
                                       / EnergyController.TimeToRestoreOneEnergyInSeconds;
            var offlineEnergyGain = (int)Math.Floor(offlineEnergyGainRaw);

            Money = gameplayData.Money;
            EnergyController.LastEnergyChangingTime = lastEnergyChangingTime;

            EnergyController.SetEnergy(Math.Clamp(
                gameplayData.CurrentEnergy + offlineEnergyGain,
                0,
                EnergyController.MaxStartEnergy));

            LoadShelterData();
        }

        private void LoadShelterData()
        {
            var shelterData = SaveManager.Instance.LoadOrDefault(new ShelterData(), $"Sh-{CurrentShelter}");

            OpenedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(shelterData.OpenedLevelsDictionaryJSonFormat);
            CompletedLevels =
                JsonConvert.DeserializeObject<Dictionary<int, bool>>(shelterData.CompletedLevelsDictionaryJSonFormat);
        }

        public void OnLevelCompleted()
        {
            CompletedLevels.Add(CurrentLevel, true);
            SaveShelterData();
        }

        public void CloseCurrentLevel()
        {
            if (OpenedLevels.ContainsKey(CurrentLevel))
                OpenedLevels[CurrentLevel] = false;
            SaveShelterData();
        }

        public void OpenAllPossibleLevels()
        {
            var levelData = GameDataHelper.AllLevelData;

            var currentLevelData = levelData
                .Where(i => i.CurrentLevelIndex == CurrentLevel &&
                            i.CurrentShelterIndex == CurrentShelter).ToList();
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

            SaveShelterData();
        }

        private void SaveGameplayData()
        {
            SaveManager.Instance.Save(new GameplayData(Instance));
        }

        private void SaveShelterData()
        {
            SaveManager.Instance.Save(new ShelterData(Instance),
                $"Sh-{Instance.CurrentShelter}");
        }

        public void ChangeLevel(int lvlIndex)
        {
            if (CurrentLevel == lvlIndex) return;

            CurrentLevel = lvlIndex;
            SaveShelterData();

            LevelChanged?.Invoke(_currentLevel);
        }

        public bool ChangeShelter(int shelterIndex)
        {
            if (CurrentShelter == shelterIndex) return false;

            CurrentShelter = shelterIndex;
            
            LoadShelterData();
            //SaveShelterData();

            ShelterChanged?.Invoke(CurrentShelter);
            return true;
        }

        private void OnApplicationQuit()
        {
            SaveGameplayData();
            SaveShelterData();
            MergeController.Instance.SaveMField();
        }
    }
}