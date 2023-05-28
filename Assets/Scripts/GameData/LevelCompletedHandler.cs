using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GameData
{
    public class LevelCompletedHandler : MonoBehaviour
    {
        private UnityAction OnOpenMenu;

        public void Initialize(UnityAction onOpenMenu)
        {
            OnOpenMenu = onOpenMenu;
        }

        public void OpenMenu()
        {
            OnOpenMenu();
        }
    }
}
