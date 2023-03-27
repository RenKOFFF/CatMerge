using UnityEngine;

namespace GameData
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Money { get; private set; }

        public void AddMoney(int amount)
        {
            Money += amount;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
