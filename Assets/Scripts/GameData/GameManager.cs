using System;
using System.Collections.Generic;
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
        public static GameManager Instance { get; private set; }
        public static EnergyController EnergyController;
        public bool IsGeneratorSpawned => GeneratorController.Instance.IsGeneratorSpawned;

        public int Money { get; private set; }
        public int Energy => EnergyController.CurrentEnergy;
        public Dictionary<int, bool> OpenedLevels;
        
        public int CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }
        public event Action<int> LevelChanged;
        
        private int _currentLevel;

        public void AddMoney(int amount)
        {
            Money += amount;
            SaveGameplayData();
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

            Money = data.Money;
            EnergyController.SetEnergy(data.CurrentEnergy);
            OpenedLevels = JsonConvert.DeserializeObject<Dictionary<int, bool>>(data.OpenedLevelsDictionaryJSonFormat);
            CurrentLevel = data.CurrentLevel;
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