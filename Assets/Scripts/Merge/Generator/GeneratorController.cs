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

            var mergeItem = MergeController.Instance.SpawnCells[generatorCellIndex].GetComponentInChildren<MergeItem>();
            if (mergeItem.TrySetData(_generatorsData[i], false))
                GeneratorCellIndex = generatorCellIndex;
        }
    }

    public int GetEmptyCellIndex()
    {
        var shakedSpawnCellsArray = ShakeArray<MergeCell>.Shake(MergeController.Instance.SpawnCells);
        
        int count = 50;
        while (count > 0)
        {
            var i = Random.Range(0, MergeController.Instance.SpawnCells.Length);

            var isEmpty = MergeController.Instance.SpawnCells[i].GetComponentInChildren<MergeItem>().IsEmpty;
            if (isEmpty) return i;
            count--;
        }

        throw new Exception("Empty");
    }
}
