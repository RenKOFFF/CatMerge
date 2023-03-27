using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Merge
{
    [CreateAssetMenu(menuName = "Custom/ClickableMergeItem")]
    public class ClickableMergeItemData : MergeItemData
    {
        [SerializeField] private MergeItemData[] _spawnItems;

        public void Spawn()
        {
            try
            {
                var cellIndex = GeneratorController.Instance.GetEmptyCellIndex();
                var item = MergeController.Instance.SpawnCells[cellIndex].GetComponentInChildren<MergeItem>();

                item.TrySetData(_spawnItems[Random.Range(0, _spawnItems.Length)], false);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}