using GameData;
using Merge;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    public void LoadLevel(int lvl)
    {
        GameManager.Instance.ChangeLevel(lvl);
        MergeController.Instance.LoadLevel();
    }
}