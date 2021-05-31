using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    Game_Manager gM;
    Player player;
    public AudioClip customMainMusic;

    public GameObject[] disableOnLoad;

    [Header("GameWin Screen Stuff")]
    public bool useGameWinStuff;
    public TextMeshProUGUI totalTimeText;
    public GameObject gameWinObject;

    public AudioClip resetProgSND;
    private void Awake()
    {
        gM = Game_Manager.Instance;
        player = FindObjectOfType<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (customMainMusic == null)
        {
            Audio_Manager.Instance.ResetToOriginalMusic(true, true);
        } else
        {
            Audio_Manager.Instance.ChangeMainMusic(customMainMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useGameWinStuff)
        {
            UpdateGameWinScreen();
        }
        gameWinObject.SetActive(AllLevelsCompleted());
    }

    public void LoadLevel(int levelID)
    {
        StartCoroutine(LoadAsync(levelID));
        for (int i = 0; i < disableOnLoad.Length; i++)
        {
            disableOnLoad[i].SetActive(false);
        }
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            int loadPercent = Mathf.RoundToInt(progress * 100);


            yield return null;
        }
        
    }
    public bool AllLevelsCompleted()
    {
        bool allComplete = true;
        for (int i = 0; i < Game_Manager.Instance.levelStatus.Length; i++)
        {
            if (!Game_Manager.Instance.levelStatus[i])
            {
                allComplete = false;
            }
        }
        return allComplete;
    }
    public void UpdateGameWinScreen()
    {
        float totalGameTime = 0;
        for (int i = 0; i < Game_Manager.Instance.bestLevelTimes.Length; i++)
        {
            totalGameTime += Game_Manager.Instance.bestLevelTimes[i];
        }
        totalTimeText.text = "Total Completion Time:\n" + FormatTime(totalGameTime);
    }
    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void ResetGameProgress()
    {
        if (resetProgSND != null)
        {
            Audio_Manager.Instance.PlayFanfare(resetProgSND);
        }
        Game_Manager.Instance.ResetSaveData();
    }
}
