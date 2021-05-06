using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public GameObject playerObject;

    public float alertTimer = 5.0f;
    public float alertTime;

    public float deathAlertRange = 5.0f;
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
        enemyAnim.SetFloat("Speed", movingSpeed);
        enemyAnim.SetBool("Dead", dead);

        if (!dead)
        {
            if (state == Enemy_Manager.EnemyState.Alert)
            {
                if (alertTime < alertTimer)
                {
                    alertTime += Time.deltaTime;
                } else
                {
                    state = Enemy_Manager.EnemyState.Idle;
                    alertTime = 0;
                    eManager.GameOver();
                    
                }
            }
        } else
        {
            alertTime = 0;
        }
    }

    public void EnemyHit()
    {
        enemyAnim.SetTrigger("Hit");
    }
    public void EnemyDeath()
    {
        dead = true;
        enemyAnim.SetTrigger("Death");
        state = Enemy_Manager.EnemyState.Dead;
        //eManager.enemies.Remove(this);
        if (!eManager.defeatedEnemies.Contains(this))
        {
            eManager.defeatedEnemies.Add(this);
        }
        AlertEnemiesArrowDirection(transform.position + targetInfo.hitFromDirection * 10f);
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
        if (state != Enemy_Manager.EnemyState.Alert)
        {
            if (willPatrol)
            {
                ChangeSuspicion(suspicionAmount);
                suspiciousArea = searchArea;

                state = Enemy_Manager.EnemyState.Search;
            }
        }
    }

    public void PlayerSpotted()
    {
        state = Enemy_Manager.EnemyState.Alert;
    }

    public void AlertEnemiesArrowDirection(Vector3 areaToSearch)
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < deathAlertRange)
            {
                allEnemies[i].StartSearch(areaToSearch, 100);
            }
        }
    }

    
}
