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

    public float enemyNearbyRange = 2.0f;

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
        canSummonCrossbow = gM.canSummonCrossbow;
        crossbowObject.SetActive(crossbowToggle);

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
        EnemiesNearby();
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
}
