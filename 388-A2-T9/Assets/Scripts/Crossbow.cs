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
    [Header("Aiming")]
    public LineRenderer aimLine;
    public bool aiming;
    private Arrow_Target lockedTarget;
    public float aimDistance = 20.0f;
    public Material aimLocked;
    public Material aimMiss;
    public Material aimOutOfRange;

    public bool autoReload;

    public LayerMask targetLayer;


    [Header("Controls")]
    public ControllerManager.Buttons reloadBtn;
    public ControllerManager.Buttons shootBtn;
    public ControllerManager.Buttons highlightBtn;
    private Highlight[] highlightables;
    private float highlightRange = 15.0f;
    private bool isHighlighting;
    private float highlightTimer = 5.0f;
    private float hTime;

    [Header("Audio Related")]
    public AudioSource audioSource;
    public AudioClip arrowLoadedSND;
    public AudioClip arrowFiredSND;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoadedArrow();
        ShootCheck();
        ReloadCheck();
        HighlightCheck();
    }

    private void UpdateLoadedArrow()
    {
        loadedArrow.SetActive(arrowLoaded);
    }
    private void ShootCheck()
    {
        AimCheck();
        if (ControllerManager.ButtonPressCheck(shootBtn))
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
        if (ControllerManager.ButtonPressCheck(reloadBtn))
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
                    if (arrowLoadedSND != null)
                    {
                        audioSource.clip = arrowLoadedSND;
                        audioSource.Play();
                    }
                }
                
            }
        }
        if (ControllerManager.ButtonUpCheck(reloadBtn))
        {
            if (!arrowLoaded)
            {
                if (rTimer > 0)
                {
                    rTimer = 0;
                    //Debug.Log("RELOAD INTERUPTED");
                }
            }
        }
        reloadTimerFill.fillAmount = rTimer / reloadTime;
    }
    public void ShootArrow()
    {
        arrowLoaded = false;
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPos.position;
        arrow.transform.rotation = arrowSpawnPos.rotation;

        arrow.GetComponent<Crossbow_Arrow>().SetTarget(lockedTarget);
        if (arrowFiredSND != null)
        {
            audioSource.clip = arrowFiredSND;
            audioSource.Play();
        }
        //Debug.Log("Shot Arrow");
    }

    public void AimCheck()
    {
        if (arrowLoaded)
        {
            aiming = ControllerManager.ButtonTouchCheck(shootBtn);
            aimLine.gameObject.SetActive(aiming);
            aimLine.SetPosition(0, arrowSpawnPos.transform.position);
            aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance);

            if (aiming)
            {
                RaycastHit hit;
                if (Physics.Raycast(arrowSpawnPos.transform.position, arrowSpawnPos.transform.forward, out hit, Mathf.Infinity, targetLayer, QueryTriggerInteraction.Collide))
                {
                    Arrow_Target tar = hit.collider.GetComponent<Arrow_Target>();
                    if (tar != null && tar.health > 0 && Vector3.Distance(arrowSpawnPos.transform.position, hit.point) > aimDistance)
                    {
                        //Valid target, but not in range
                        aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance + arrowSpawnPos.position);
                        aimLine.material = aimOutOfRange;
                        lockedTarget = null;
                    }
                    else if (tar != null && tar.health > 0 && Vector3.Distance(arrowSpawnPos.transform.position, hit.point) <= aimDistance)
                    {
                        //Valid target and in range
                        aimLine.SetPosition(1, tar.transform.position);
                        aimLine.material = aimLocked;
                        lockedTarget = tar;

                    }
                    else if (tar == null || tar.health <= 0)
                    {
                        //Target not visible
                        aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance + arrowSpawnPos.position);
                        aimLine.material = aimMiss;
                        lockedTarget = null;
                    }
                } else
                {
                    // If raycast didn't hit anything
                    aimLine.SetPosition(1, arrowSpawnPos.transform.forward * aimDistance + arrowSpawnPos.position);
                    aimLine.material = aimMiss;
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
            if (ControllerManager.ButtonUpCheck(highlightBtn))
            {
                hTime = 0;
                isHighlighting = false;
                StopHighlight();
            }
        } else
        {
            if (ControllerManager.ButtonPressCheck(highlightBtn))
            {
                isHighlighting = true;
                HighlightObjects();
            }
        }
        
    }
    public void HighlightObjects()
    {
        //Find all Highlightable Objects
        highlightables = FindObjectsOfType<Highlight>();
        for (int i = 0; i < highlightables.Length; i++)
        {
            //Check the distance to the object, if within the highlight range it will highlight it
            if (Vector3.Distance(transform.position, highlightables[i].transform.position) < highlightRange * 2)
            {
                highlightables[i].StartHighlight();
            }
        }
    }
    public void StopHighlight()
    {
        //Find all highlightable objects and tell them to stop highlighting
        highlightables = FindObjectsOfType<Highlight>();
        for (int i = 0; i < highlightables.Length; i++)
        {
            highlightables[i].StopHighlight();
        }
    }
}
