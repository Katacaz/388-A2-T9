using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Detection : MonoBehaviour
{
    Enemy enemyBase;
    public GameObject searchImage;
    public Image searchFill;

    public GameObject alertImage;
    public Image alertFill;

    bool searching;
    bool alert;

    public GameObject enemyHead;
    public GameObject lookTarget;
    Vector3 lookTargetDefaultPos;
    //Quaternion defaultHeadRot;

    [Header("Detection Parameters")]
    public bool looking = true;
    public float detectionRange = 10.0f;
    public float peripheralAngle = 45.0f;

    //public Transform target;
    public bool playerSeen;

    [Header("Raycast Related")]
    public LayerMask playerLayer;

    [Header("Animator Relatted")]
    public Animator lookAnim;

    Vector3 playerLastSeen = new Vector3();

    private void Awake()
    {
        enemyBase = GetComponent<Enemy>();
        //defaultHeadRot = enemyHead.transform.rotation;
        lookTargetDefaultPos = lookTarget.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        searching = (enemyBase.state == Enemy_Manager.EnemyState.Search);
        lookAnim.SetBool("Searching", ((searching && looking) || (enemyBase.state == Enemy_Manager.EnemyState.Idle && looking)));
        if (searching)
        {
            lookAnim.SetFloat("SearchSpeed", 1.0f);
        } else
        {
            lookAnim.SetFloat("SearchSpeed", 0.5f);
        }
        alert = (enemyBase.state == Enemy_Manager.EnemyState.Alert);

        if (!alert && !enemyBase.dead)
        {
            searchImage.SetActive(searching);
        }
        else
        {
            searchImage.SetActive(false);
        }
        alertImage.SetActive(alert);

        searchFill.fillAmount = (enemyBase.suspicion / 100f);
        Color searchColor = searchFill.color;
        searchColor.a = (enemyBase.suspicion / 100f);
        searchFill.color = searchColor;

        alertFill.fillAmount = (enemyBase.alertProgress / 100f);
        Color alertColor = alertFill.color;
        alertColor.a = (enemyBase.alertProgress / 100f);
        alertFill.color = alertColor; ;


        if (enemyBase.dead)
        {
            looking = false;
        } else
        {
            SmokeScreenCheck();
        }
        if (looking)
        {
            //Move about the look target position
            if (playerSeen)
            {
                lookTarget.transform.position = enemyBase.playerObject.transform.position;
            } else
            {
                if (alert)
                {
                    enemyBase.StartSearch(playerLastSeen, 100);
                }
            }
            if (enemyBase.suspicion > 0)
            {
                /*Vector3 lookDir = (transform.position - enemyBase.suspiciousArea);
                lookDir.x = 0;
                lookDir.z = 0;
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f * Time.deltaTime);*/
                if (Vector3.Distance(this.transform.position, enemyBase.suspiciousArea) > 3)
                {
                    lookTarget.transform.position = enemyBase.suspiciousArea;
                }
                
            }
            //Check if player is in range of viewing angle
            Vector3 playerDirection = (enemyBase.playerObject.transform.position - enemyHead.transform.position);
            float angle = Vector3.Angle(playerDirection, enemyHead.transform.forward);
            float playerDistance = Vector3.Distance(enemyHead.transform.position, enemyBase.playerObject.transform.position);
            //Check if player is in range
            if (playerDistance < detectionRange)
            {
                //Check if player is within the vision angle
                if (angle < peripheralAngle)
                {
                    //Check if actually visible
                    RaycastHit hit;
                    if (Physics.Raycast(enemyHead.transform.position, playerDirection, out hit, detectionRange, playerLayer, QueryTriggerInteraction.Collide))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            //Player hit with raycast
                            Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.red);
                            if (!PlayerInSmoke())
                            {
                                SawPlayer();
                            }
                            
                            //Debug.Log("Player in range, in angle and seen");
                        }
                        else
                        {
                            Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.yellow);
                            //Debug.Log("Player in range, in angle BUT not seen");
                            playerSeen = false;
                        }
                    }
                    else
                    {
                        //Nothing hit by raycast
                        //Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.grey);
                        playerSeen = false;
                    }

                }
                else
                {
                    Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.green);
                    playerSeen = false;
                    //Debug.Log("Player in range, not in angle");
                }
            } else
            {
                //Player out of detection Range
                playerSeen = false;
            }


        } else
        {
            playerSeen = false;
        }
    }
    public void SawPlayer()
    {
        playerSeen = true;
        playerLastSeen = enemyBase.playerObject.transform.position;
        enemyBase.PlayerSpotted();
    }
    public void SmokeScreenCheck()
    {
        bool inSmoke = false;
        SmokeScreen[] smokescreens = FindObjectsOfType<SmokeScreen>();
        for (int i = 0; i < smokescreens.Length; i++)
        {
            if (Vector3.Distance(this.transform.position, smokescreens[i].transform.position) <= smokescreens[i].smokeRadius)
            {
                //This enemy is within range of the smoke screen
                if (smokescreens[i].smokeActivated)
                {
                    //Smoke was activated
                    inSmoke = true;
                }
            }
        }

        if (inSmoke)
        {
            looking = false;
        } else
        {
            looking = true;
        }
    }
    public bool PlayerInSmoke()
    {
        bool playerInSmoke = false;
        SmokeScreen[] smokescreens = FindObjectsOfType<SmokeScreen>();
        for (int i = 0; i < smokescreens.Length; i++)
        {
            if (Vector3.Distance(enemyBase.playerObject.transform.position, smokescreens[i].transform.position) <= smokescreens[i].smokeRadius)
            {
                //This enemy is within range of the smoke screen
                if (smokescreens[i].smokeActivated)
                {
                    //Smoke was activated
                    playerInSmoke = true;
                }
            }
        }
        return playerInSmoke;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyHead.transform.position, detectionRange);
        Gizmos.DrawLine(enemyHead.transform.position, (enemyHead.transform.position + enemyHead.transform.forward * detectionRange));
        //Gizmos.color = Color.green;
        
    }    
}
