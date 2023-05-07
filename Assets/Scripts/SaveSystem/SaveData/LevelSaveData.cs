using System;
using System.Collections.Generic;
using Merge;
using Merge.Energy;
using Newtonsoft.Json;
using SaveSystem.SaveData;

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

            Dictionary<int, string> cellsDictionary = new();

            for (int i = 0; i < mergeController.MergeCells.Length; i++)
            {
                if (!mergeController.MergeCells[i].MergeItem.IsEmpty)
                {
                    cellsDictionary.Add(i, mergeController.MergeCells[i].MergeItem.MergeItemData.name);
                }
                else
                {
                    cellsDictionary.Add(i, "empty");
                }
            }
            
            CellsDictionaryJSonFormat = JsonConvert.SerializeObject(cellsDictionary);
        }

        public LevelSaveData(int mergeCellsLength)
        {
            IsGeneratorSpawned = false;
            Dictionary<int, string> cellsDictionary = new();

            for (int i = 0; i < mergeCellsLength; i++)
            {
                cellsDictionary.Add(i, "empty");
            }
            
            CellsDictionaryJSonFormat = JsonConvert.SerializeObject(cellsDictionary);
        }
    }
}