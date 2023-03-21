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

        var mergeItem = MergeController.Instance.MergeCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
        if (mergeItem.TrySetData(_data, false))
            GeneratorCellIndex = generatorCellIndex;
    }
    
    public int GetEmptyCellIndex()
    {
        int count = 50;
        while (count > 0)
        {
            var i = Random.Range(0, MergeController.Instance.MergeCells.Length);
            
            var isEmpty = MergeController.Instance.MergeCells[i].GetComponentInChildren<MergeItem>().IsEmpty;
            if (isEmpty) return i;
            count--;
        }

        throw new Exception("Empty");
    }
}
