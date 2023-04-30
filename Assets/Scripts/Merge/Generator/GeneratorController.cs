using System;
using GameData;
using Merge;
using SaveSystem;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorController : MonoBehaviour
{
    [SerializeField] private MergeItemData[] _generatorsData;

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
        
        for (int i = 0; i < _generatorsData.Length; i++)
        {
            var generatorCellIndex = MergeController.GetEmptyCellIndex();

            if (generatorCellIndex == -1)
            {
                Debug.Log("can't spawn generator");
                return;
            }

            var mergeItem = MergeController.Instance.MergeCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
            if (mergeItem.TrySetData(_generatorsData[i], false))
                GeneratorCellIndex = generatorCellIndex;
        }

        SetIsGeneratorSpawned(true);
    }

    public void SetIsGeneratorSpawned(bool value)
    {
        IsGeneratorSpawned = value;
        SaveManager.Instance.Save(new LevelSaveData(MergeController.Instance, value), GameManager.Instance.CurrentLevel.ToString());
    }
}
