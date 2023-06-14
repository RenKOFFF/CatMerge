using System;
using System.Linq;
using GameData;
using SaveSystem;
using UnityEngine;

namespace Merge.Generator
{
    public class GeneratorController : MonoBehaviour
    {
        [SerializeField] private GeneratorsData[] _generators;
    
        public int GeneratorCellIndex { get; private set; }
        public static GeneratorController Instance { get; private set; }
        public bool IsGeneratorSpawned { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnGenerator()
        {
            if (IsGeneratorSpawned) return;
            var generators = _generators
                .Where(g => g.ShelterIndex == GameManager.Instance.CurrentShelter)
                .SelectMany(d => d.Data)
                .ToArray();

            for (int i = 0; i < generators.Length; i++)
            {
                var generatorCellIndex = MergeController.GetEmptyCellIndex();

                if (generatorCellIndex == -1)
                {
                    Debug.Log("can't spawn generator");
                    return;
                }

                var mergeItem = MergeController.Instance.MergeCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
                if (mergeItem.TrySetData(generators[i], false))
                {
                    GeneratorCellIndex = generatorCellIndex;
                    MergeController.Instance.UpdateUnlockedComplexityLevel(mergeItem.MergeItemData);
                }
            }

            SetIsGeneratorSpawned(true);
        }

        public void SetIsGeneratorSpawned(bool value)
        {
            if (IsGeneratorSpawned == value || GameManager.Instance.CurrentLevel == 0) return;

            IsGeneratorSpawned = value;
            SaveManager.Instance.Save(new LevelSaveData(MergeController.Instance, value),
                $"Sh-{GameManager.Instance.CurrentShelter}-Lvl-{GameManager.Instance.CurrentLevel}");
        }

        [Serializable]
        public class GeneratorsData
        {
            public int ShelterIndex = 1;
            public MergeItemData[] Data;
        }
    }
}