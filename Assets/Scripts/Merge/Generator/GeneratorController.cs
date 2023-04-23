using System;
using Merge;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
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
            var generatorCellIndex = GetEmptyCellIndex();

            if (generatorCellIndex == -1)
            {
                Debug.Log("can't spawn generator");
                return;
            }
            
            var mergeItem = MergeController.Instance.MergeCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
            if (mergeItem.TrySetData(_generatorsData[i], false))
                GeneratorCellIndex = generatorCellIndex;
        }

        IsGeneratorSpawned = true;
    }

    public void SetIsGeneratorSpawned(bool value)
    {
        IsGeneratorSpawned = value;
    }

    public int GetEmptyCellIndex()
    {
        var mergeCellsLength = MergeController.Instance.MergeCells.Length;
        var spawnCellsIndexesArray = new int[mergeCellsLength];

        for (int i = 0; i < mergeCellsLength; i++)
        {
            spawnCellsIndexesArray[i] = i;
        }
        
        var shakedSpawnCellsIndexesArray =  ShakeArray<int>.Shake(spawnCellsIndexesArray);
        var shakedIndex = 0;
        
        for (int i = 0; i < shakedSpawnCellsIndexesArray.Length; i++)
        {
            shakedIndex = shakedSpawnCellsIndexesArray[i];
            
            var isEmpty = MergeController.Instance.MergeCells[shakedIndex].GetComponentInChildren<MergeItem>().IsEmpty;
            if (isEmpty) return shakedIndex;
        }

        Debug.Log("Not empty cells");
        return -1;
    }
}
