using System;
using Merge.Energy;
using SaveSystem;
using UnityEngine;

namespace GameData
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static EnergyController EnergyController;
        public bool IsGeneratorSpawned => GeneratorController.Instance.IsGeneratorSpawned;

        public int Money { get; private set; }
        public int Energy => EnergyController.CurrentEnergy;

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
        }
        
        private void SaveGameplayData()
        {
            SaveManager.Instance.Save(new GameplayData(Instance));
        }
    }
}