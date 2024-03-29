using GameData;
using Merge;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    public void LoadLevel(int lvl)
    {
        GameManager.Instance.ChangeLevel(lvl);
        LoadCurrentLevel();
    }
    
    public void LoadCurrentLevel()
    {
        MergeController.Instance.LoadLevel();
    }

    public void ChangeShelter(int shelterIndex)
    {
        GameManager.Instance.ChangeShelter(shelterIndex);
    }
}