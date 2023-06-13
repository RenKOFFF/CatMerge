using Orders;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuProgressPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void ShowProgressByLevelIndex(int shelter, int level)
        {
            _text.text = $"{OrderManager.Instance.GetOrderProgressInLevel(shelter, level)}" +
                         $"/{OrderManager.GetOrdersNeededToCompleteLevelCount(level)}";
        }
    }
}