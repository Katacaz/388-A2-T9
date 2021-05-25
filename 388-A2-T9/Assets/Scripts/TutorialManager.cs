using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    Game_Manager gM;
    public int firstLevelIndex = 1;

    public int tutorialProgress = 0;
    private bool beginNextStep;

    public TextMeshProUGUI speechBubbleText;
    [Header("Step Text")]
    [TextArea]
    public string equipCrossbowMessage = "[Message for Equiping the Crossbow]";
    [TextArea]
    public string revealTargetMessage = "[Message for Revealing Targets]";
    [TextArea]
    public string shootingMessage = "[Message for Shooting the Targets]";
    [TextArea]
    public string equipTeleportMessage = "[Message for Equiping the Teleporter]";
    [TextArea]
    public string teleportingMessage = "[Message for Teleporting]";
    [TextArea]
    public string endTutorialMessage = "[Message for finishing the tutorial]";
    [TextArea]
    public string reloadMessage = "[Message for reloading]";

    public Player player;
    public Crossbow crossbow;
    public Player_Teleporter teleportTool;

    private void Awake()
    {
        gM = Game_Manager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        ProgressTutorial(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (beginNextStep)
        {
            beginNextStep = false;
            switch (tutorialProgress)
            {
                case (1):
                    // Step 1 - Equip Crossbow
                    Step1();
                    break;
                case (2):
                    // Step 2 - Reveal Targets
                    Step2();
                    break;
                case (3):
                    // Step 3 - Shoot Target
                    Step3();
                    break;
                case (4):
                    // Step 4 - Reload
                    Step4();
                    break;
                case (5):
                    //Step 5 - Equip Teleporter
                    Step5();
                    break;
                case (6):
                    //Step 6 - Teleport
                    Step6();
                    break;
                case (7):
                    //Step 7 - Finish Tutorial
                    Step7();
                    break;

                    
            }
        }

        //Step Completion Checks
        switch (tutorialProgress)
        {
            case (1):
                // Step 1 - Equip Crossbow
                if (player.crossbowToggle)
                {
                    ProgressTutorial(2);
                }
                break;
            case (2):
                // Step 2 - Reveal Targets
                if (ControllerManager.ButtonPressCheck(crossbow.highlightBtn))
                {
                    ProgressTutorial(3);
                }
                break;
            case (3):
                // Step 3 - Shoot Target
                break;
            case (4):
                // Step 4 - Reload
                if (crossbow.arrowLoaded)
                {
                    ProgressTutorial(5);
                }
                break;
            case (5):
                //Step 5 - Equip Teleporter
                if (teleportTool.canTeleport)
                {
                    ProgressTutorial(6);
                }
                break;
            case (6):
                //Step 6 - Teleport
                break;


        }
    }


    public void StartGame()
    {
        TutorialCompleted();
        //gM.SaveGameProgress();
        Game_Manager.Instance.SaveGameProgress();
        SceneManager.LoadScene(firstLevelIndex);
    }

    public void TutorialCompleted()
    {
        Game_Manager.Instance.tutorialComplete = true;
        //gM.tutorialComplete = true;
    }

    public void ProgressTutorial(int stage)
    {
        tutorialProgress = stage;
        beginNextStep = true;
    }
    public void TargetShot()
    {
        if (tutorialProgress == 3)
        {
            ProgressTutorial(4);
        }
    }
    public void PlayerTeleported()
    {
        if (tutorialProgress == 6)
        {
            ProgressTutorial(7);
        }
    }
    private void Step1()
    {
        //Step 1 - Equip Crossbow
        speechBubbleText.text = equipCrossbowMessage;

    }
    private void Step2()
    {
        //Step 2 - Reveal Targets
        speechBubbleText.text = revealTargetMessage;
    }
    private void Step3()
    {
        // Step 3 - Shoot Target
        speechBubbleText.text = shootingMessage;

    }
    private void Step4()
    {
        //Step 4 - Reload
        speechBubbleText.text = reloadMessage;
    }
    private void Step5()
    {
        //Step 5 - Equip Teleporter
        speechBubbleText.text = equipTeleportMessage;

    }
    private void Step6()
    {
        //Step 6 - Teleport
        speechBubbleText.text = teleportingMessage;
    }
    private void Step7()
    {
        //Step 7 - Finish Tutorial
        speechBubbleText.text = endTutorialMessage;
    }
}
