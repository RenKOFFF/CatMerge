using Orders;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuProgressPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        public void ShowProgressByLevelIndex(int level)
        {
            _text.text = $"{OrderManager.Instance.GetOrderProgressInLevel(level)}" +
                         $"/{OrderManager.GetOrdersNeededToCompleteLevelCount(level)}";
        }
    }
}