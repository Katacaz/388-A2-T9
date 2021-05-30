using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    Enemy_Manager eManager;
    public Arrow_Target targetInfo;
    public Animator enemyAnim;

    public bool dead;

    public Enemy_Manager.EnemyState state;

    public float movingSpeed;
    public bool willPatrol;
    [Range(0, 100)]
    public float suspicion;
    public Vector3 suspiciousArea = new Vector3();
    [Range(0, 100)]
    public float alertProgress;
    public float alertPerSecond = 10f; //10 seconds to fully alert

    public GameObject playerObject;

    public float alertTimer = 5.0f;
    public float alertTime;

    public float deathAlertRange = 10.0f;

    public GameObject deadObject;
    [Header("Held Object Related")]
    public Rig armRig;
    public GameObject heldObject;

    [Header("Enemy Info")]
    public Sprite icon;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSND;
    public AudioClip searchSND;
    public AudioClip alertSND;
    public AudioSource stepAudioSource;
    public AudioClip[] stepSND;

    private float alertSNDdelay = 5.0f;
    private float alertSNDtimer;
    private void Awake()
    {
        eManager = FindObjectOfType<Enemy_Manager>();
        eManager.enemies.Add(this);
        //Since the player has the enemy manager on them, just set the player to the manager
        playerObject = FindObjectOfType<Player>().gameObject;
        suspiciousArea = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (willPatrol)
        {
            state = Enemy_Manager.EnemyState.Patrol;
        }
    }

    // Update is called once per frame
    void Update()
    {
        deadObject.SetActive(dead);
        if (heldObject != null)
        {
            heldObject.SetActive(!dead);
            if (dead)
            {
                armRig.weight = 0;
            } else
            {
                armRig.weight = 1;
            }
        } else
        {
            armRig.weight = 0;
        }
        GetComponent<NavMeshAgent>().enabled = !dead;
        enemyAnim.SetFloat("Speed", movingSpeed);
        enemyAnim.SetBool("Dead", dead);

        if (!dead)
        {
            if (state == Enemy_Manager.EnemyState.Alert)
            {
                eManager.playerSpotted = true;
                if (alertProgress < 100)
                {
                    alertProgress += Time.deltaTime * alertPerSecond;
                }
            } else
            {
                eManager.playerSpotted = false;
                //If not alerted, decrease alert progress until 0 constantly
                if (alertProgress > 0)
                {
                    alertProgress -= Time.deltaTime * (alertPerSecond * 0.5f);
                }
            }
        } else
        {
            alertTime = 0;
        }

        if (eManager.playerSpotted)
        {
            if (alertSNDtimer > alertSNDdelay)
            {
                alertSNDtimer = alertSNDdelay;
            } else if (alertSNDtimer == alertSNDdelay)
            {
                alertSNDtimer -= Time.deltaTime;
                OVRInput.SetControllerVibration(1f, 0.5f, OVRInput.Controller.RTouch);
                OVRInput.SetControllerVibration(1f, 0.5f, OVRInput.Controller.LTouch);
                if (alertSND != null)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = alertSND;
                        audioSource.Play();
                    }
                }
            } else if (alertSNDtimer > 0 && alertSNDtimer < alertSNDdelay)
            {
                alertSNDtimer -= Time.deltaTime;
            } else
            {
                alertSNDtimer = 0;
            }
        } else
        {
            if (alertSNDtimer < alertSNDdelay)
            {
                alertSNDtimer += Time.deltaTime;
            } else if (alertSNDtimer <= 0)
            {
                alertSNDtimer = alertSNDdelay;
            }
        }
    }

    public void EnemyHit()
    {
        enemyAnim.SetTrigger("Hit");
        StartSearch(transform.position + targetInfo.hitFromDirection * 10f, 100f);
    }
    public void EnemyDeath()
    {
        if (deathSND != null)
        {
            audioSource.clip = deathSND;
            audioSource.Play();
        }
        if (!dead)
        {
            dead = true;
            enemyAnim.SetTrigger("Death");
            state = Enemy_Manager.EnemyState.Dead;
            //eManager.enemies.Remove(this);
            if (!eManager.defeatedEnemies.Contains(this))
            {
                eManager.defeatedEnemies.Add(this);
            }
            AlertEnemiesArrowDirection(transform.position + targetInfo.hitFromDirection * 10f, 100f);
            eManager.playerSpotted = false;
        }
    }

    public void ChangeSuspicion(float amount)
    {
        suspicion += amount;
        if (suspicion < 0)
        {
            suspicion = 0;
        } else if (suspicion > 100)
        {
            suspicion = 100;
        }
    }

    public void StartSearch(Vector3 searchArea, float suspicionAmount)
    {
        if (!dead)
        {
            if (willPatrol)
            {
                //If blinded they will not be given the area to look, but still become suspicious
                if (GetComponent<Enemy_Detection>().looking)
                {
                    suspiciousArea = searchArea;
                } else
                {
                    suspiciousArea = this.transform.position;
                }

                ChangeSuspicion(suspicionAmount);
                state = Enemy_Manager.EnemyState.Search;
            }
            else
            {
                AlertEnemiesArrowDirection(searchArea, suspicionAmount);
            }
            if (searchSND != null)
            {
                audioSource.clip = searchSND;
                audioSource.Play();
            }
        }
    }

    public void PlayerSpotted()
    {
        state = Enemy_Manager.EnemyState.Alert;

        
    }
    public void PlayerCaught()
    {
        eManager.GameOver();
    }

    public void AlertEnemiesArrowDirection(Vector3 areaToSearch, float suspicionAmount)
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i] != this)
            {
                if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < deathAlertRange)
                {
                    allEnemies[i].StartSearch(areaToSearch, suspicionAmount);
                }
            }
        }
    }

    public void Step()
    {
        if (stepSND != null)
        {
            stepAudioSource.clip = stepSND[Random.Range(0, stepSND.Length)];
            stepAudioSource.Play();
        }
    }
}
