using TMPro;
using UnityEngine;

namespace GameData
{
    public class ShowPlayerMoney : MonoBehaviour
    {
        private TMP_Text _tmpText;

        private void Awake()
        {
            _tmpText = GetComponent<TMP_Text>() ?? GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            _tmpText.text = GameManager.Instance.Money.ToString();
        }
    }
}
