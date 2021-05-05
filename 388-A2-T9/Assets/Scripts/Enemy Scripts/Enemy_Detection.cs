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

        alertFill.fillAmount = (enemyBase.alertTime / enemyBase.alertTimer);
        Color alertColor = alertFill.color;
        alertColor.a = (enemyBase.alertTime / enemyBase.alertTimer);
        alertFill.color = alertColor; ;


        if (enemyBase.dead)
        {
            looking = false;
        }
        if (looking)
        {
            //Move about the look target position
            if (playerSeen)
            {
                lookTarget.transform.position = enemyBase.playerObject.transform.position;
            }
            else
            {
                if (enemyBase.suspicion > 0)
                {
                    lookTarget.transform.position = enemyBase.suspiciousArea;
                }
                else
                {
                    lookTarget.transform.localPosition = lookTargetDefaultPos;
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
                                SawPlayer();
                                //Debug.Log("Player in range, in angle and seen");
                            } else
                            {
                                Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.yellow);
                                //Debug.Log("Player in range, in angle BUT not seen");
                            }
                        }
                        else
                        {
                            //Nothing hit by raycast
                            //Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.grey);
                        }
                        
                    }
                    else
                    {
                        Debug.DrawLine(enemyHead.transform.position, enemyBase.playerObject.transform.position, Color.green);
                        //Debug.Log("Player in range, not in angle");
                    }
                }
            }


        }
    }
    public void SawPlayer()
    {
        playerSeen = true;
        enemyBase.PlayerSpotted();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyHead.transform.position, detectionRange);
        //Gizmos.DrawLine(enemyHead.transform.position, (enemyHead.transform.position + enemyHead.transform.forward * detectionRange));
        //Gizmos.color = Color.green;
    }
}
