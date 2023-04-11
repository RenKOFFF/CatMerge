using Merge;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeCell : MonoBehaviour, IDropHandler
{
    [SerializeField] private MergeItem mergeItem;
    [SerializeField] private GameObject isUsedForOrderFlag;

    public MergeItem MergeItem => mergeItem;

    public void OnDrop(PointerEventData eventData)
    {
        MergeController.Instance.OnEndDrag(MergeItem);
    }

    private void Update()
    {
        isUsedForOrderFlag.SetActive(MergeItem.IsUsedForOrder && !MergeItem.IsMoving);
    }
}
