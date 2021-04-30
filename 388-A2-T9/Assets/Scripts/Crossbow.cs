using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crossbow : MonoBehaviour
{
    [Header("Arrow Info")]
    public GameObject arrowPrefab;
    public Transform arrowSpawnPos;
    public bool arrowLoaded;
    public float reloadTime = 2.0f;
    private float rTimer;
    public Image reloadTimerFill;

    public GameObject loadedArrow;

    public LineRenderer aimLine;
    public bool aiming;
    private Arrow_Target lockedTarget;

    public float aimDistance = 20.0f;

    public bool autoReload;

    public LayerMask targetLayer;


    [Header("Controls")]
    public Buttons reloadBtn;
    public Buttons shootBtn;
    public Buttons highlightBtn;
    private Highlight[] highlightables;
    private float highlightRange = 15.0f;
    private bool isHighlighting;
    private float highlightTimer = 5.0f;
    private float hTime;
    public enum Buttons
    {
        A,
        B,
        X,
        Y,
        LTrigger,
        LButton,
        RTrigger,
        RButton
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        UpdateLoadedArrow();
        ShootCheck();
        ReloadCheck();
        HighlightCheck();
    }
    private bool ButtonPressCheck(Buttons button)
    {
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.Get(OVRInput.RawButton.A);
                break;
            case Buttons.B:
                status = OVRInput.Get(OVRInput.RawButton.B);
                break;
            case Buttons.X:
                status = OVRInput.Get(OVRInput.RawButton.X);
                break;
            case Buttons.Y:
                status = OVRInput.Get(OVRInput.RawButton.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.Get(OVRInput.RawButton.LHandTrigger);
                break;
            case Buttons.RTrigger:
                status = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
                break;
        }
        return status;
    }
    private bool ButtonTouchCheck(Buttons button)
    {
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.Get(OVRInput.RawTouch.A);
                break;
            case Buttons.B:
                status = OVRInput.Get(OVRInput.RawTouch.B);
                break;
            case Buttons.X:
                status = OVRInput.Get(OVRInput.RawTouch.X);
                break;
            case Buttons.Y:
                status = OVRInput.Get(OVRInput.RawTouch.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.Get(OVRInput.RawTouch.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.Get(OVRInput.RawTouch.LTouchpad);
                break;
            case Buttons.RTrigger:
                status = OVRInput.Get(OVRInput.RawTouch.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.Get(OVRInput.RawTouch.RTouchpad);
                break;
        }
        return status;
    }
    private void ButtonUpCheck()
    {

    }
    private void ButtonDownCheck()
    {

    }
    private void UpdateLoadedArrow()
    {
        loadedArrow.SetActive(arrowLoaded);
    }
    private void ShootCheck()
    {
        AimCheck();
        if (ButtonPressCheck(shootBtn))
        {
            if (arrowLoaded)
            {
                ShootArrow();
            }
            
            
        }
    }
    private void ReloadCheck()
    {
        reloadTimerFill.gameObject.SetActive(!arrowLoaded);
        if (ButtonPressCheck(reloadBtn))
        {
            if (!arrowLoaded)
            {
                if (autoReload)
                {
                    rTimer = reloadTime;
                }
                if (rTimer < reloadTime)
                {
                    rTimer += Time.deltaTime;
                } else
                {
                    arrowLoaded = true;
                    rTimer = 0;
                    //Debug.Log("Arrow Successfully Reloaded");
                }
                
            }
        }
        /*if (OVRInput.GetUp(reloadButton))
        {
            if (!arrowLoaded)
            {
                if (rTimer > 0)
                {
                    rTimer = 0;
                    //Debug.Log("RELOAD INTERUPTED");
                }
            }
        }*/
        reloadTimerFill.fillAmount = rTimer / reloadTime;
    }
    public void ShootArrow()
    {
        arrowLoaded = false;
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPos.position;
        arrow.transform.rotation = arrowSpawnPos.rotation;

        arrow.GetComponent<Crossbow_Arrow>().SetTarget(lockedTarget);
        //Debug.Log("Shot Arrow");
    }

    public void AimCheck()
    {
        if (arrowLoaded)
        {
            aiming = ButtonTouchCheck(shootBtn);
            aimLine.gameObject.SetActive(aiming);
            aimLine.SetPosition(0, arrowSpawnPos.transform.position);
            aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance);
            
            if (aiming)
            {
                RaycastHit hit;
                if (Physics.Raycast(arrowSpawnPos.transform.position, arrowSpawnPos.transform.forward, out hit, aimDistance, targetLayer, QueryTriggerInteraction.Collide))
                {
                    Arrow_Target tar = hit.collider.GetComponent<Arrow_Target>();
                    if (tar != null)
                    {
                        // Target in range
                        aimLine.SetPosition(1, tar.transform.position);
                        lockedTarget = tar;
                    }
                    else
                    {
                        //Target not visible
                        aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance + arrowSpawnPos.position);
                        lockedTarget = null;
                    }
                } else
                {
                    // If raycast didn't hit anything
                    aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance + arrowSpawnPos.position);
                    lockedTarget = null;
                }
            }
        } else
        {
            aimLine.gameObject.SetActive(false);
        }
    }
    public void HighlightCheck()
    {
        if (isHighlighting)
        {
            if (hTime < highlightTimer)
            {
                hTime += Time.deltaTime;
            } else
            {
                hTime = 0;
                isHighlighting = false;
                StopHighlight();
            }
        } else
        {
            if (ButtonPressCheck(highlightBtn))
            {
                isHighlighting = true;
                HighlightObjects();
            }
        }
        
    }
    public void HighlightObjects()
    {
        highlightables = FindObjectsOfType<Highlight>();
        for (int i = 0; i < highlightables.Length; i++)
        {
            highlightables[i].StartHighlight();
        }
    }
    public void StopHighlight()
    {
        
        highlightables = FindObjectsOfType<Highlight>();
        for (int i = 0; i < highlightables.Length; i++)
        {
            highlightables[i].StopHighlight();
        }
    }
}
