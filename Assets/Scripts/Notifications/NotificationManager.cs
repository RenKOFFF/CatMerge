using System;
using Merge.Energy;
using Unity.Notifications.Android;
using UnityEngine;

namespace Notifications
{
    public class NotificationManager : MonoBehaviour
    {
        void Start()
        {
            CreateNotificationChanel();
            //EnergyController.Instance.OnEnergyIsFull += SendNotification;
        }

        private void OnDestroy()
        {
            //EnergyController.Instance.OnEnergyIsFull -= SendNotification;
        }

        private void CreateNotificationChanel()
        {
            var chanel = new AndroidNotificationChannel
            {
                Id = "chanel_id",
                Name = "Chanel",
                Importance = Importance.High,
                Description = "Test",
                EnableVibration = true,
                VibrationPattern = new long[]{100,100,100},
                LockScreenVisibility = LockScreenVisibility.Public,
                EnableLights = true
            };
            AndroidNotificationCenter.RegisterNotificationChannel(chanel);
        }

        private void SendNotification()
        {
            var notification = CreateAndroidNotification();

            AndroidNotificationCenter.SendNotification(notification, "chanel_id");
        }

        private AndroidNotification CreateAndroidNotification()
        { 
            var notification = new AndroidNotification()
            {
                Title = "Хочешь потрогать кисок? Энергия восстановилась!",
                Text = "Чего же ты ждешь. Вперед к кискам",
                LargeIcon = "icon_0",
                FireTime = DateTime.Now.AddSeconds(3f)
            };
            return notification;
        }
    }
}
