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

        public int Money { get; private set; }
        public int Energy => EnergyController.CurrentEnergy;

        public void AddMoney(int amount)
        {
            Money += amount;
            
            SaveManager.Instance.Save(new GameplayData(Instance));
        }

        public void AddEnergy(int amount)
        {
            EnergyController.Instance.AddEnergy(amount);
            SaveManager.Instance.Save(new GameplayData(Instance));
        }

        public void SpendEnergy()
        {
            EnergyController.Instance.SpendEnergy();
            SaveManager.Instance.Save(new GameplayData(Instance));
        }

        private void Awake()
        {
            Instance = this;
            EnergyController = GetComponent<EnergyController>();
        }

        private void Start()
        {
            var data = SaveManager.Instance.Load(new GameplayData());

            Money = data.Money;
            EnergyController.SetEnergy(data.CurrentEnergy);
        }
    }
}