using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    Game_Manager gM;

    private void Awake()
    {
        gM = FindObjectOfType<Game_Manager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
}
