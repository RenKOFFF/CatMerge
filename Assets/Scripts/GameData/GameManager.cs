using System;
using Merge.Energy;
using UnityEngine;

namespace GameData
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static EnergyController EnergyController;

        public int Money { get; private set; }

        public void AddMoney(int amount)
        {
            Money += amount;
        }

        public void AddEnergy(int amount)
        {
            EnergyController.Instance.AddEnergy(amount);
        }

        public void SpendEnergy()
        {
            EnergyController.Instance.SpendEnergy();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            EnergyController = GetComponent<EnergyController>();
        }
    }
}