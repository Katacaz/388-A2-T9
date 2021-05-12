using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> defeatedEnemies = new List<Enemy>();

    public bool gameWin;
    public GameObject gameWinUI;

    public GameObject remainingEnemiesFrame;
    public bool remainingEnemiesToggle = true;
    public OVRInput.Button remainingEnemiesBtn;
    //public ControllerManager.Buttons remainingEnemiesBtn;
    public GameObject enemyIconPrefab;
    public TextMeshProUGUI remainingEnemiesText;
    public int remainingEnemies;

    public int sceneIndexToLoadOnWin = 0;
    public int sceneIndexToLoadOnLoss = 0;

    public bool playerSpotted;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Search,
        Alert,
        Dead
    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        SetUpRemainingEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        remainingEnemiesFrame.SetActive(remainingEnemiesToggle);
        if (OVRInput.GetDown(remainingEnemiesBtn))//ControllerManager.ButtonDownCheck(remainingEnemiesBtn))
        {
            ToggleRemainingEnemies(!remainingEnemiesToggle);
        }
        UpdateRemainingEnemies();

        if (AllEnemiesDefeated())
        {
            SceneManager.LoadScene(sceneIndexToLoadOnWin);
        }
        gameWinUI.SetActive(playerSpotted);
        
    }
    public void ToggleRemainingEnemies(bool state)
    {
        remainingEnemiesToggle = state;
    }
    public void SetUpRemainingEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject iconObj = Instantiate(enemyIconPrefab, remainingEnemiesFrame.transform);
            EnemyIcon icon = iconObj.GetComponent<EnemyIcon>();
            icon.SetUpIcon(enemies[i]);
        }
    }
    public void UpdateRemainingEnemies()
    {
        remainingEnemies = (enemies.Count - defeatedEnemies.Count);
        remainingEnemiesText.text = "Remaining Enemies: " + remainingEnemies;
    }


    public bool AllEnemiesDefeated()
    {
        return (defeatedEnemies.Count >= enemies.Count);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Debug.Log("Player Spotted, GAME OVER");
        SceneManager.LoadScene(sceneIndexToLoadOnLoss);
    }
}
