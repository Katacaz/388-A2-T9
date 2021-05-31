using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private Game_Manager gM;
    public int levelID;
    private GameObject playerObject;

    public int levelSelectSceneID = 1;
    public int gameOverSceneID = 1;
    private bool levelCompleted;
    [Header("Game Win Screen Vars")]
    public GameObject gameWinObject;
    public TextMeshProUGUI loadingText;
    public Slider loadingBar;
    public ControllerManager.Buttons continueButton = ControllerManager.Buttons.A;
    public TextMeshProUGUI levelTimeText;
    public TextMeshProUGUI bestLevelTimeText;
    private bool levelLoading;

    public float levelTime;
    private bool timerActive;
    public float bestTime;

    public AudioClip gameWinFanfareAudio;
    public AudioClip customLevelMainMusic;
    public AudioClip customCombatMusic;
    private void Awake()
    {
        gM = Game_Manager.Instance;
        playerObject = FindObjectOfType<Player>().gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelStart();
    }

    // Update is called once per frame
    void Update()
    {
        GameWinScreenUpdater();
        
        if (timerActive)
        {
            levelTime += Time.deltaTime;
        }
        if (levelCompleted)
        {
            if (!levelLoading)
            {
                if (ControllerManager.ButtonPressCheck(continueButton))
                {
                    levelLoading = true;
                    LoadLevel(levelSelectSceneID);
                }
            }
        }
    }
    public void LevelStart()
    {
        timerActive = true;
        bestTime = Game_Manager.Instance.bestLevelTimes[levelID];

        if ( customLevelMainMusic != null)
        {
            Audio_Manager.Instance.ChangeMainMusic(customLevelMainMusic);
        }
        if (customCombatMusic != null)
        {
            Audio_Manager.Instance.ChangeCombatMusic(customCombatMusic);
        }
    }
    public void LevelCompleted()
    {
        timerActive = false;
        levelCompleted = true;
        Audio_Manager.Instance.PlayFanfare(gameWinFanfareAudio);
        //Compare scores
        if (levelTime < bestTime || bestTime == 0)
        {
            Game_Manager.Instance.bestLevelTimes[levelID] = levelTime;
        }

        Game_Manager.Instance.levelStatus[levelID] = true;
        //gM.levelStatus[levelID] = true;
        Game_Manager.Instance.SaveGameProgress();
        //gM.SaveGameProgress();
    }
    public void LevelLost()
    {
        SceneManager.LoadScene(gameOverSceneID);
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
        
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingBar.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            int loadPercent = Mathf.RoundToInt(progress * 100);
            loadingText.text = "Loading: " + loadPercent + "%";
            yield return null;
        }
    }

    public void GameWinScreenUpdater()
    {
        gameWinObject.transform.position = (playerObject.transform.position + (playerObject.transform.forward * 2));
        gameWinObject.SetActive(levelCompleted);
        bestLevelTimeText.text = "Best Time: " + FormatTime(bestTime);
        levelTimeText.text = "Current Time: " + FormatTime(levelTime);
        //Show if score is beaten
        if (levelTime < bestTime)
        {
            //Score beated
            levelTimeText.color = Color.green;
        } else
        {
            levelTimeText.color = Color.red;
        }

        if (!levelLoading)
        {
            loadingBar.gameObject.SetActive(false);
            loadingText.text = "Press B to Continue";
        }

        
    }
    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
