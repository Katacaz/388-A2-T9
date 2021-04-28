using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public List<Enemy> enemies;

    public List<Enemy> defeatedEnemies;

    public bool gameWin;
    public GameObject gameWinUI;

    public TextMeshProUGUI remainingEnemiesText;
    public int remainingEnemies;

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
        gameWinUI.SetActive(AllEnemiesDefeated());
        
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
}
