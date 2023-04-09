using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
