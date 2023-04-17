using System;
using GameData;
using Merge;
using Merge.Energy;

namespace SaveSystem
{
    [Serializable]
    public class GameplayData
    {
        public int CurrentEnergy;
        public int Money;

        public GameplayData(GameManager gameManager)
        {
            CurrentEnergy = gameManager.Energy;
            Money = gameManager.Money;
        }
        
        /// <summary>
        /// Default values
        /// </summary>
        public GameplayData()
        {
            CurrentEnergy = GameManager.EnergyController.MaxStartEnergy;
            Money = 0;
        }
    }

    [Serializable]
    public class LevelSaveData
    {
        public int LevelNumber;
        public MergeCell[] mergeCells;

        public LevelSaveData(MergeController mergeController)
        {
            LevelNumber = 1;
            mergeCells = mergeController.MergeCells;
        }
    }
}