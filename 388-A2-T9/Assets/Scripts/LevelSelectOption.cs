using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectOption : MonoBehaviour
{
    Game_Manager gM;
    LevelSelectManager lsM;
    public int levelID;
    public int levelSceneIndex;
    public bool levelUnlocked;
    public GameObject model;
    public bool[] prereqLevels;
    private bool levelComplete;
    public GameObject levelCompleteObject;

    private float bestTime;
    public TextMeshProUGUI bestTimeText;
    private void Awake()
    {
        gM = Game_Manager.Instance;
        lsM = FindObjectOfType<LevelSelectManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPrereqConditions();
        CheckLevelComplete();
        CheckBestTime();
        
    }
    void CheckPrereqConditions()
    {
        if (!levelUnlocked)
        {
            bool tempUnlocked = false;
            for (int i = 0; i < prereqLevels.Length; i++)
            {
                //Check if level is required
                if (prereqLevels[i])
                {
                    //Check if level was completed
                    if (Game_Manager.Instance.levelStatus[i])
                    {
                        tempUnlocked = true;
                    } else
                    {
                        tempUnlocked = false;
                    }
                }
            }
            levelUnlocked = tempUnlocked;
        }

        model.SetActive(levelUnlocked);
        
    }
    void CheckBestTime()
    {
        bestTimeText.gameObject.SetActive(levelComplete);
        bestTime = Game_Manager.Instance.bestLevelTimes[levelID];
        bestTimeText.text = "Best Time: " + FormatTime(bestTime);
    }
    void CheckLevelComplete()
    {
        levelComplete = Game_Manager.Instance.levelStatus[levelID];
        levelCompleteObject.SetActive(levelComplete);
    }

    public void StartLevel()
    {
        lsM.LoadLevel(levelSceneIndex);
    }

    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
