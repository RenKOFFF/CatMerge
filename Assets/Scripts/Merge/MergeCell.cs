using Merge;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeCell : MonoBehaviour, IDropHandler
{
    [SerializeField] private MergeItem mergeItem;
    [SerializeField] private GameObject isUsedForOrderFlag;
    [SerializeField] private GameObject _isGeneratorMark;

    public MergeItem MergeItem => mergeItem;

    public void OnDrop(PointerEventData eventData)
    {
        MergeController.Instance.OnDrop(MergeItem);
    }

    private void Update()
    {
        isUsedForOrderFlag.SetActive(MergeItem.IsUsedForOrder && !MergeItem.IsMoving);
        _isGeneratorMark.SetActive(MergeItem.IsGenerator && MergeItem.MergeItemData.ComplexityLevel > 1 && !MergeItem.IsMoving);
    }
}
