using System.Collections;
using System.Collections.Generic;
using Merge;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeCell : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var itemOnThisCell = GetComponentInChildren<MergeItem>();
        MergeController.Instance.OnEndDrag(itemOnThisCell);
    }
}
