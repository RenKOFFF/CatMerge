using System;
using System.Collections;
using System.Collections.Generic;
using Merge;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorController : MonoBehaviour
{
    [SerializeField] private MergeItemData _data;
    
    public Transform GeneratorCellLocation { get; private set; }
    
    private void Start()
    {
        SpawnGenerator();
    }

    private void SpawnGenerator()
    {
        var rndCellIndex = GetRandomCellLocation();

        var mergeItem = MergeController.Instance.SpawnCells[rndCellIndex].GetComponentInChildren<MergeItem>();
        mergeItem.SetData(_data);
    }

    private int GetRandomCellLocation()
    {
        var rndCellIndex = Random.Range(0, MergeController.Instance.SpawnCells.Length);
        return rndCellIndex;
    }
}
