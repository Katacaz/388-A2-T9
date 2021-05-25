using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Game_Manager gM;
    public int levelID;

    public int levelSelectSceneID = 1;
    public int gameOverSceneID = 1;
    private void Awake()
    {
        gM = Game_Manager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelCompleted()
    {
        Game_Manager.Instance.levelStatus[levelID] = true;
        //gM.levelStatus[levelID] = true;
        Game_Manager.Instance.SaveGameProgress();
        //gM.SaveGameProgress();
        SceneManager.LoadScene(levelSelectSceneID);
    }
    public void LevelLost()
    {
        SceneManager.LoadScene(gameOverSceneID);
    }
}
