using System;
using System.Collections.Generic;
using Merge;
using Merge.Energy;
using Newtonsoft.Json;

namespace SaveSystem
{
    [Serializable]
    public class LevelSaveData
    {
        public bool IsGeneratorSpawned;
        public string CellsDictionaryJSonFormat;

        public LevelSaveData(MergeController mergeController, bool isGeneratorSpawned)
        {
            IsGeneratorSpawned = isGeneratorSpawned;

            Dictionary<int, string> CellsDictionary = new();

            for (int i = 0; i < mergeController.MergeCells.Length; i++)
            {
                if (!mergeController.MergeCells[i].MergeItem.IsEmpty)
                {
                    CellsDictionary.Add(i, mergeController.MergeCells[i].MergeItem.MergeItemData.name);
                }
                else
                {
                    CellsDictionary.Add(i, "empty");
                }
            }

            CellsDictionaryJSonFormat = JsonConvert.SerializeObject(CellsDictionary);
        }

        public LevelSaveData(int MergeCellsLength)
        {
            IsGeneratorSpawned = false;
            Dictionary<int, string> CellsDictionary = new();

            for (int i = 0; i < MergeCellsLength; i++)
            {
                CellsDictionary.Add(i, "empty");
            }

            CellsDictionaryJSonFormat = JsonConvert.SerializeObject(CellsDictionary);
        }
    }
}