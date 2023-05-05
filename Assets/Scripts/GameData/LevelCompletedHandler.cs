using UnityEngine;

namespace GameData
{
    public class LevelCompletedHandler : MonoBehaviour
    {
        public void OpenMenu()
        {
            var canvas =  GameObject.FindGameObjectWithTag(GameConstants.Tags.Canvas);
            var menuCanvas =  GameObject.FindGameObjectWithTag(GameConstants.Tags.MenuCanvas);

            canvas.SetActive(false);
            menuCanvas.SetActive(true);
            Destroy(gameObject);
        }
    }
}
