using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Teleporter : MonoBehaviour
{
    public bool canTeleport;
    public Transform teleportAimPos;
    public GameObject teleportProp;
    public LayerMask teleportLayer;
    public bool aiming;
    public float aimDistance;
    public LineRenderer aimLine;
    public Material aimLocked;
    public Material aimMissed;

    public Vector3 teleportOffset;
    public OVRInput.RawButton tpButton;
    public OVRInput.RawTouch tpButtonTouch;
    public ControllerManager.Buttons teleportButton;
    public ControllerManager.Buttons startTeleportButton;

    private Teleport_Point targetPoint;

    private GameObject playerObject;

    private GameObject activeAimEffect;
    public GameObject teleportLockedEffectPrefab;
    public GameObject teleportToEffectPrefab;
    public GameObject teleportFromEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTeleportProp();
        ReloadCheck();
        TeleportCheck();
    }

    public void UpdateTeleportProp()
    {
        teleportProp.SetActive(canTeleport);
    }
    public void ReloadCheck()
    {
        if (!canTeleport)
        {
            if (ControllerManager.ButtonDownCheck(startTeleportButton))
            {
                canTeleport = true;
            }
        } else
        {
            if (ControllerManager.ButtonDownCheck(startTeleportButton))
            {
                canTeleport = false;
            }
        }
    }
    public void TeleportCheck()
    {
        Aim();
        if (OVRInput.Get(tpButton))//ControllerManager.ButtonPressCheck(teleportButton))
        {
            if (canTeleport)
            {
                Teleport();
            }
        }

    }
    public void Teleport()
    {
        if (targetPoint != null)
        {
            Vector3 teleportTo = targetPoint.transform.position + teleportOffset;
            //Debug.Log("Teleporting to: " + teleportTo);
            if (teleportFromEffectPrefab != null)
            {
                GameObject eF = Instantiate(teleportFromEffectPrefab);
                eF.transform.position = playerObject.transform.position;
                Destroy(eF, 5.0f);
            }
            playerObject.transform.position = teleportTo;
            if (teleportToEffectPrefab != null)
            {
                GameObject eT = Instantiate(teleportFromEffectPrefab);
                eT.transform.position = playerObject.transform.position;
                Destroy(eT, 5.0f);
            }
            /*if (activeAimEffect != null)
            {
                Destroy(activeAimEffect);
                activeAimEffect = null;
            }*/
            //canTeleport = false;
        }
    }
    public void Aim()
    {
 
        if (canTeleport)
        {
            //aiming = ControllerManager.ButtonTouchCheck(teleportButton);
            aiming = OVRInput.Get(tpButtonTouch);
            aiming = true;
            aimLine.gameObject.SetActive(aiming);
            aimLine.SetPosition(0, teleportAimPos.transform.position);
            aimLine.SetPosition(1, teleportAimPos.transform.forward * aimDistance);

            if (aiming)
            {
                RaycastHit hit;
                if (Physics.Raycast(teleportAimPos.transform.position, teleportAimPos.transform.forward, out hit, Mathf.Infinity, teleportLayer, QueryTriggerInteraction.Collide))
                {
                    Teleport_Point tar = hit.collider.GetComponent<Teleport_Point>();
                    //Arrow_Target tar = hit.collider.GetComponent<Arrow_Target>();
                    if (tar != null && Vector3.Distance(teleportAimPos.transform.position, hit.point) > aimDistance)
                    {
                        //Valid target, but not in range
                        aimLine.SetPosition(1, teleportAimPos.transform.forward * aimDistance + teleportAimPos.position);
                        aimLine.material = aimMissed;
                        targetPoint = null;
                        if (activeAimEffect != null)
                        {
                            Destroy(activeAimEffect);
                            activeAimEffect = null;
                        }
                    }
                    else if (tar != null && Vector3.Distance(teleportAimPos.transform.position, hit.point) <= aimDistance)
                    {
                        //Valid target and in range
                        aimLine.SetPosition(1, tar.transform.position);
                        aimLine.material = aimLocked;
                        targetPoint = tar;
                        if (activeAimEffect != null)
                        {
                            if (teleportLockedEffectPrefab != null)
                            {
                                GameObject e = Instantiate(teleportLockedEffectPrefab);
                                e.transform.position = tar.transform.position;
                                activeAimEffect = e;
                            }
                        }

                    }
                    else if (tar == null)
                    {
                        //Target not visible
                        aimLine.SetPosition(1, teleportAimPos.transform.forward * aimDistance + teleportAimPos.position);
                        aimLine.material = aimMissed;
                        targetPoint = null;
                        if (activeAimEffect != null)
                        {
                            Destroy(activeAimEffect);
                            activeAimEffect = null;
                        }
                    }
                }
                else
                {
                    // If raycast didn't hit anything
                    aimLine.SetPosition(1, teleportAimPos.transform.forward * aimDistance + teleportAimPos.position);
                    aimLine.material = aimMissed;
                    targetPoint = null;
                    if (activeAimEffect != null)
                    {
                        Destroy(activeAimEffect);
                        activeAimEffect = null;
                    }
                }
            }
        }
        else
        {
            aimLine.gameObject.SetActive(false);
        }
    }
}
