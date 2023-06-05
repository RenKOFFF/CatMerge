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
            if (EnergyController.Instance.CurrentEnergy <= 0) return;

            var line = GetRandomLine();
            if (line == -1)
            {
                Debug.Log("Line is empty");
                return;
            }

            var itemIndex = GetRandomItemIndexByGeneratorAge(line);
            if (itemIndex == -1)
            {
                Debug.Log("itemIndex is empty");
                return;
            }

            var spawnItem = _lines.GenerateLine[line].ItemData[itemIndex];
            if (MergeController.Instance.SpawnItem(spawnItem))
            {
                GameManager.Instance.SpendEnergy();
            }
        }

        private int GetRandomLine()
        {
            if (_lines.GenerateLine.Length == 0) return -1;

            return Random.Range(0, _lines.GenerateLine.Length);
        }

        private int GetRandomItemIndexByGeneratorAge(int line)
        {
            List<float> chances = new();

            for (int i = 0; i < _lines.GenerateLine[line].ItemData.Length; i++)
            {
                chances.Add(_lines.GenerateLine[line].ItemData[i].SpawnChance.Evaluate(_age));
            }

            if (chances.Count == 0)
                return -1;

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