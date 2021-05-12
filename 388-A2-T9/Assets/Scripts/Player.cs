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
    public OVRInput.Button crossbowToggleBtn;
    //public ControllerManager.Buttons crossbowToggleBtn;
    public GameObject crossbowSummonEffect;
    public GameObject crossbowDesummonEffect;

    [Header("Grabber Related")]
    public OVRGrabber rightHandGrabber;
    public OVRGrabber leftHandGrabber;

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
}
