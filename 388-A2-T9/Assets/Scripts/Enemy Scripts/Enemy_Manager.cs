using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> defeatedEnemies = new List<Enemy>();

    public bool gameWin;
    public GameObject gameWinUI;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRemainingEnemies();

        if (AllEnemiesDefeated())
        {
            SceneManager.LoadScene(sceneIndexToLoadOnWin);
        }
        gameWinUI.SetActive(playerSpotted);
        
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
