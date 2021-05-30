using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private static Game_Manager instance;
    public static Game_Manager Instance {  get { return instance; } }
    public bool canSummonCrossbow;
    public bool canUseTeleporter;

    public bool tutorialComplete;
    [Tooltip("Array of levels, bool representing if completed or not")]
    public bool[] levelStatus;
    public float[] bestLevelTimes;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            instance = this;
        }
        Game_Manager.Instance.LoadGameProgress();
    }
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
            bestLevelTimes[i] = data.bestLevelTimes[i];
        }
    }

    public void ResetSaveData()
    {
        tutorialComplete = false;
        int totalLevels = levelStatus.Length;
        levelStatus = new bool[totalLevels];
        bestLevelTimes = new float[totalLevels];
        SaveSystem.SaveGameInfo(this);
    }
}
