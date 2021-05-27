using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelSaveData
{
    public bool tutorialComplete;
    public bool[] levelsComplete;
    public float[] bestLevelTimes;

    public LevelSaveData (Game_Manager gM)
    {
        tutorialComplete = gM.tutorialComplete;
        levelsComplete = new bool[gM.levelStatus.Length];
        bestLevelTimes = new float[gM.bestLevelTimes.Length];

        for (int i = 0; i < levelsComplete.Length; i++)
        {
            levelsComplete[i] = gM.levelStatus[i];
            bestLevelTimes[i] = gM.bestLevelTimes[i];
        }
    }
}
