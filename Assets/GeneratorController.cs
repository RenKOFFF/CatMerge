using System;
using System.Collections;
using System.Collections.Generic;
using Merge;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorController : MonoBehaviour
{
    [SerializeField] private MergeItemData _data;

    public int GeneratorCellIndex { get; private set; }

    private void Start()
    {
        SpawnGenerator();
    }

    private void SpawnGenerator()
    {
        var rndCellIndex = GetRandomCellLocation();

        var mergeItem = MergeController.Instance.SpawnCells[rndCellIndex].GetComponentInChildren<MergeItem>();
        if (mergeItem.TrySetData(_data, false))
            GeneratorCellIndex = rndCellIndex;
    }

    private int GetRandomCellLocation()
    {
        var rndCellIndex = Random.Range(0, MergeController.Instance.SpawnCells.Length);
        return rndCellIndex;
    }
}