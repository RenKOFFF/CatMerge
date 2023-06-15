using UnityEngine;

namespace Components
{
    public class UrlHandler : MonoBehaviour
    {
        [SerializeField] private string url = "https://youtu.be/dQw4w9WgXcQ";

        public void OpenUrl()
        {
            Application.OpenURL(url);
        }
    }
}
