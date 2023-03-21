using Merge;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeCell : MonoBehaviour, IDropHandler
{
    [SerializeField] private MergeItem mergeItem;

    public MergeItem MergeItem => mergeItem;

    public void OnDrop(PointerEventData eventData)
    {
        MergeController.Instance.OnEndDrag(MergeItem);
    }
}
