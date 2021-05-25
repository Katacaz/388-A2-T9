using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Game_Manager gM;
    [Header("Summoning Crossbow Related")]
    public bool canSummonCrossbow;
    public GameObject crossbowObject;
    public bool crossbowToggle;
    public OVRInput.RawButton crossbowToggleBtn;
    //public ControllerManager.Buttons crossbowToggleBtn;
    public GameObject crossbowSummonEffect;
    public GameObject crossbowDesummonEffect;

    [Header("Grabber Related")]
    public OVRGrabber rightHandGrabber;
    public OVRGrabber leftHandGrabber;

    [Header("TeleportRelated")]
    public bool canSummonTeleporter;
    private GameObject teleporterToolObject;
    public Player_Teleporter tpTool;

    public float enemyNearbyRange = 2.0f;

    [Header("Grapple Tool stuff")]
    public float grappleSpeed = 5.0f;
    public GameObject leftGrappleObjectPrefab;
    public GameObject leftGrappleOrigin;
    public LineRenderer leftGrappleLine;
    private float leftGrappleDistance;
    private bool leftGrappleActive;
    private bool leftGrappleGrabbedObject;
    public GameObject rightGrappleObjectPrefab;
    public GameObject rightGrappleOrigin;
    public LineRenderer rightGrappleLine;
    private float rightGrappleDistance;
    private bool rightGrappleActive;
    private bool rightGrappleGrabbedObject;


    private void Awake()
    {
        gM = Game_Manager.Instance;
        teleporterToolObject = tpTool.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canSummonCrossbow = Game_Manager.Instance.canSummonCrossbow;
        canSummonTeleporter = Game_Manager.Instance.canUseTeleporter;
        //canSummonCrossbow = gM.canSummonCrossbow;
        //canSummonTeleporter = gM.canUseTeleporter;
        crossbowObject.SetActive(crossbowToggle);

        teleporterToolObject.SetActive(canSummonTeleporter);

        //Disable being able to grab stuff if the crossbow is active
        if (crossbowToggle)
        {
            rightHandGrabber.enabled = false;
            rightHandGrabber.GetComponent<SphereCollider>().enabled = false;
        } else
        {
            rightHandGrabber.enabled = true;
            rightHandGrabber.GetComponent<SphereCollider>().enabled = true;
        }
        if (canSummonCrossbow)
        {
            if (OVRInput.GetDown(crossbowToggleBtn))//ControllerManager.ButtonDownCheck(crossbowToggleBtn))
            {
                SetCrossbowActive(!crossbowToggle);
            }
        }
        //If the teleporter item is being used (Kunai has appeared), you cannot grab objects.
        if (tpTool.canTeleport)
        {
            leftHandGrabber.enabled = false;
            leftHandGrabber.GetComponent<SphereCollider>().enabled = false;
        } else
        {
            leftHandGrabber.enabled = true;
            leftHandGrabber.GetComponent<SphereCollider>().enabled = true;
        }
        EnemiesNearby();

        //If attempting to grab with left hand
        if (leftHandGrabber.enabled)
        {
            if (OVRInput.Get(OVRInput.RawButton.LHandTrigger))
            {
                if (leftHandGrabber.grabbedObject == null)
                {
                    //Debug.Log("Tried Grabbing, got nothing.");
                    if (!leftGrappleGrabbedObject)
                    {
                        LeftGrappleShoot();
                    } 
                    else
                    {
                        LeftGrappleRetract();
                    }
                }
                else
                {
                    //Debug.Log("GOT SOMETHING");
                }
            }
        }
        if (rightHandGrabber.enabled)
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                if (rightHandGrabber.grabbedObject == null)
                {
                    // Didn't grab anything with right hand
                } else
                {
                    //Grabbed something with right hand.
                }
            }
        }

    }

    public void SetCrossbowActive(bool state)
    {
        crossbowToggle = state;
        if (crossbowToggle)
        {
            //Crossbow turned on
            if (crossbowSummonEffect != null)
            {
                GameObject effect = Instantiate(crossbowSummonEffect);
                effect.transform.position = crossbowObject.transform.position;
                Destroy(effect, 5.0f);
            }
        } else
        {
            //Crossbow turned off
            if (crossbowDesummonEffect != null)
            {
                GameObject effect = Instantiate(crossbowDesummonEffect);
                effect.transform.position = crossbowObject.transform.position;
                Destroy(effect, 5.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerOnPlayerTouch tar = other.GetComponent<TriggerOnPlayerTouch>();
        if (tar != null)
        {
            //Target hit
            tar.TriggeredByPlayer();
        }
        
    }

    public void EnemiesNearby()
    {
        int nearby = 0;
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (!allEnemies[i].dead && Vector3.Distance(transform.position, allEnemies[i].transform.position) <= enemyNearbyRange)
            {
                nearby++;
            }
        }
        
        //OVRInput.SetControllerVibration(1f, Mathf.Clamp((0.1f * nearby), 0, 1), OVRInput.Controller.RTouch);
        //OVRInput.SetControllerVibration(1f, Mathf.Clamp((0.1f * nearby), 0, 1), OVRInput.Controller.LTouch);
    }

    public void LeftGrappleShoot()
    {

    }
    public void LeftGrappleRetract()
    {

    }
}
