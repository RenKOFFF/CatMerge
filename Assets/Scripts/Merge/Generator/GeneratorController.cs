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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnGenerator();
    }

    private void SpawnGenerator()
    {
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
    }

    public int GetEmptyCellIndex()
    {
        var shakedSpawnCellsArray = ShakeArray<MergeCell>.Shake(MergeController.Instance.MergeCells);

        for (int i = 0; i < shakedSpawnCellsArray.Length; i++)
        {
            var isEmpty = MergeController.Instance.MergeCells[i].GetComponentInChildren<MergeItem>().IsEmpty;
            if (isEmpty) return i;
        }

        Debug.Log("Not empty cells");
        return -1;
    }
}
