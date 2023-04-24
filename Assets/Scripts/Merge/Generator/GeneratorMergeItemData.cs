using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Merge.Energy;
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
            if (EnergyController.Instance.CurrentEnergy > 0)
            {
                var cellIndex = MergeController.GetEmptyCellIndex();

                if (cellIndex == -1)
                {
                    Debug.Log("can't spawn");
                    return;
                }
                
                var itemOnSpecificCell =
                    MergeController.Instance.MergeCells[cellIndex].GetComponentInChildren<MergeItem>();

                var line = GetRandomLine();
                var itemIndex = GetRandomItemIndexByGeneratorAge(line);
                
                itemOnSpecificCell.TrySetData(_lines.GenerateLine[line].ItemData[itemIndex], false);
                
                GameManager.Instance.SpendEnergy();
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
