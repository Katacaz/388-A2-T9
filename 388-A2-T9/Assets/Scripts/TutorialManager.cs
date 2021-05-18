using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    Game_Manager gM;
    public int firstLevelIndex = 1;

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


    public void StartGame()
    {
        TutorialCompleted();
        gM.SaveGameProgress();
        SceneManager.LoadScene(firstLevelIndex);
    }

    public void TutorialCompleted()
    {
        gM.tutorialComplete = true;
    }
}
