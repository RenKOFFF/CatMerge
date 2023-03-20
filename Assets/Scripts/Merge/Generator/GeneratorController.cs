using System;
using Merge;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorController : MonoBehaviour
{
    [SerializeField] private MergeItemData _data;

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
        var generatorCellIndex = GetEmptyCellIndex();

        var mergeItem = MergeController.Instance.SpawnCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
        if (mergeItem.TrySetData(_data, false))
            GeneratorCellIndex = generatorCellIndex;
    }
    
    public int GetEmptyCellIndex()
    {
        for (int i = 0; i < MergeController.Instance.SpawnCells.Length; i++)
        {
            var isEmpty = MergeController.Instance.SpawnCells[i].GetComponentInChildren<MergeItem>().IsEmpty;
            if (isEmpty) return i;
        }

        throw new Exception("Empty");
    }
}