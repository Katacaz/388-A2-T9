using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    public bool canSummonCrossbow;
    public bool canUseTeleporter;

    public bool tutorialComplete;
    [Tooltip("Array of levels, bool representing if completed or not")]
    public bool[] levelStatus;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeUseCrossbow(bool value)
    {
        canSummonCrossbow = value;
    }
    public void ChangeUseTeleporter(bool value)
    {
        canUseTeleporter = value;
    }

    public void SaveGameProgress()
    {
        SaveSystem.SaveGameInfo(this);
    }

    public void LoadGameProgress()
    {
        LevelSaveData data = SaveSystem.LoadGameInfo();

        tutorialComplete = data.tutorialComplete;

        for (int i = 0; i < levelStatus.Length; i++)
        {
            levelStatus[i] = data.levelsComplete[i];
        }
    }
}
