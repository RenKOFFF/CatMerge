using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Merge.Selling
{
    public class SellButton : MonoBehaviour, IDeselectHandler
    {
        private MergeItem _sellingItem;

        public void Initialize(MergeItem sellingItem)
        {
            _sellingItem = sellingItem;
            EventSystem.current.SetSelectedGameObject(gameObject);
            gameObject.SetActive(true);
        }

        public void Sell()
        {
            _sellingItem.Sell();
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}
