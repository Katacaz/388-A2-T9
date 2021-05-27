using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialisationScene : MonoBehaviour
{
    Game_Manager gM;
    //public OVRInput.RawButton startButton;
    public ControllerManager.Buttons startButton = ControllerManager.Buttons.B;
    public int levelSelectSceneID;
    public int tutorialSceneID;

    private void Awake()
    {
        gM = Game_Manager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        Game_Manager.Instance.LoadGameProgress();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllerManager.ButtonPressCheck(startButton))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        if (!Game_Manager.Instance.tutorialComplete)
        {
            //First time starting - run tutorial
            SceneManager.LoadScene(tutorialSceneID);
        } else
        {
            SceneManager.LoadScene(levelSelectSceneID);
        }
    }
}
