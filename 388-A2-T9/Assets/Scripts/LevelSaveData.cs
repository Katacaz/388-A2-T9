using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelSaveData
{
    public bool tutorialComplete;
    public bool[] levelsComplete;

    public LevelSaveData (Game_Manager gM)
    {
        tutorialComplete = gM.tutorialComplete;
        levelsComplete = new bool[gM.levelStatus.Length];

        for (int i = 0; i < levelsComplete.Length; i++)
        {
            levelsComplete[i] = gM.levelStatus[i];
        }
    }
}
