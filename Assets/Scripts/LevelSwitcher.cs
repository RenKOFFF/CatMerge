using GameData;
using Merge;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    public void LoadLevel(int lvl)
    {
        if (GameManager.Instance.CurrentLevel == lvl) return;
        
        GameManager.Instance.ChangeLevel(lvl);
        MergeController.Instance.LoadLevel();
    }
}
