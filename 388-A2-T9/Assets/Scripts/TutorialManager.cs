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

    public GameObject speechCanvas;
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
    [TextArea]
    public string turningMessage = "[Message for turning]";
    [TextArea]
    public string enemyTrackerMessage = "[Message for using the Enemy Tracker]";

    public Player player;
    public Crossbow crossbow;
    public Player_Teleporter teleportTool;

    public GameObject controllerImage;
    private Animator controllerAnim;

    public AudioClip progressTutorialFanfareClip;

    private void Awake()
    {
        gM = Game_Manager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        controllerAnim = controllerImage.GetComponent<Animator>();
        tutorialProgress = 1;
        beginNextStep = true;
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
                    StepCrossbow();
                    break;
                case (2):
                    // Step 2 - Reveal Targets
                    StepRevealTargets();
                    break;
                case (3):
                    // Step 3 - Shoot Target
                    StepShoot();
                    break;
                case (4):
                    // Step 4 - Reload
                    StepReload();
                    break;
                case (5):
                    //Step 5 - Equip Teleporter
                    StepTeleporter();
                    break;
                case (6):
                    //Step 6 - Teleport
                    StepTeleport();
                    break;
                case (7):
                    //Step 7 - Finish Tutorial
                    StepFinish();
                    break;
                case (8):
                    //Step 8 - Turning
                    StepTurn();
                    break;
                case (9):
                    //Step 9 - Tracker
                    StepTracker();
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
                //if (ControllerManager.ButtonPressCheck(crossbow.highlightBtn))
                if (player.isHighlighting)
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
                    ProgressTutorial(8);
                }
                break;
            case (6):
                //Step 6 - Teleport
                break;
            case (8):
                //step 8 - turning
                if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > 0.5f || OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < -0.5)
                {
                    ProgressTutorial(6);
                }
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
        Audio_Manager.Instance.PlayFanfare(progressTutorialFanfareClip);
        tutorialProgress = stage;
        beginNextStep = true;
    }
    public void TargetShot()
    {
        if (tutorialProgress == 3)
        {
            ProgressTutorial(5);
        }
    }
    public void PlayerTeleported()
    {
        if (tutorialProgress == 6)
        {
            ProgressTutorial(7);
        }
    }
    private void StepCrossbow()
    {
        //Step 1 - Equip Crossbow
        controllerAnim.SetTrigger("Right_B");
        speechBubbleText.text = equipCrossbowMessage;

    }
    private void StepRevealTargets()
    {
        //Step 2 - Reveal Targets
        controllerAnim.SetTrigger("Right_A");
        speechBubbleText.text = revealTargetMessage;
    }
    private void StepShoot()
    {
        // Step 3 - Shoot Target
        controllerAnim.SetTrigger("Right_Trigger");
        speechBubbleText.text = shootingMessage;

    }
    private void StepReload()
    {
        //Step 4 - Reload
        controllerAnim.SetTrigger("Right_Grab");
        speechBubbleText.text = reloadMessage;
    }
    private void StepTeleporter()
    {
        //Step 5 - Equip Teleporter
        controllerAnim.SetTrigger("Left_X");
        speechBubbleText.text = equipTeleportMessage;

    }
    private void StepTeleport()
    {
        //Step 6 - Teleport
        controllerAnim.SetTrigger("Left_Trigger");
        speechBubbleText.text = teleportingMessage;
    }
    private void StepFinish()
    {
        //Step 7 - Finish Tutorial
        controllerImage.SetActive(false);
        speechBubbleText.text = endTutorialMessage;
    }
    private void StepTurn()
    {
        //Step 8 - Turning
        controllerAnim.SetTrigger("Right_Stick");
        speechBubbleText.text = turningMessage;
    }
    private void StepTracker()
    {
        //Step 9 - Tracker
        controllerAnim.SetTrigger("Left_Y");
        speechBubbleText.text = enemyTrackerMessage;
    }

    public void MoveMessage(Transform position)
    {
        speechCanvas.transform.position = position.position;
    }
}
