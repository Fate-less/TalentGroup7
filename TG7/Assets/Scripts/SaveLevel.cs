using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    public void SaveGame(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }
    public int LoadGame()
    {
        return PlayerPrefs.GetInt("Level", 1);
    }
}
