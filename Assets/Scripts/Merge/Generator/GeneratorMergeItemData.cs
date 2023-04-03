﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Merge.Generator
{
    [CreateAssetMenu(menuName = "Custom/GeneratorMergeItem")]
    public class GeneratorMergeItemData : MergeItemData
    {
        [SerializeField] private int _age;
        [SerializeField] private GenerateLines _lines;

        public void Spawn()
        {
            if (EnergyController.Instance.SpendEnergy())
            {
                var cellIndex = GeneratorController.Instance.GetEmptyCellIndex();
                var itemOnSpecificCell =
                    MergeController.Instance.SpawnCells[cellIndex].GetComponentInChildren<MergeItem>();

                var line = GetRandomLine();
                var itemIndex = GetRandomItemIndexByGeneratorAge(line);

                Debug.Log($"Line {line}, itemIndex: {itemIndex}");
                
                itemOnSpecificCell.TrySetData(_lines.GenerateLine[line].ItemData[itemIndex], false);
            }
        }

        private int GetRandomLine()
        {
            return Random.Range(0, _lines.GenerateLine.Length);
        }

        private int GetRandomItemIndexByGeneratorAge(int line)
        {
            List<float> chances = new();

            for (int i = 0; i < _lines.GenerateLine[line].ItemData.Length; i++)
            {
                chances.Add(_lines.GenerateLine[line].ItemData[i].SpawnChance.Evaluate(_age));
            }

            var value = Random.Range(0, chances.Sum());

            var sum = 0f;
            for (int i = 0; i < _lines.GenerateLine[line].ItemData.Length; i++)
            {
                sum += chances[i];
                if (value < sum)
                    return i;
            }

            return _lines.GenerateLine[line].ItemData.Length - 1;
        }
    }

    [Serializable]
    public class GenerateLines : List<GenerateLine>
    {
        public GenerateLine[] GenerateLine;
    }

    [Serializable]
    public class GenerateLine : List<MergeItemData>
    {
        public MergeItemData[] ItemData;
    }
}