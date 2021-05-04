using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Enemy_Manager eManager;

    public Animator enemyAnim;

    public bool dead;

    public Enemy_Manager.EnemyState state;

    public float movingSpeed;
    public bool willPatrol;
    public float suspicion;
    public Vector3 suspiciousArea = new Vector3();
    private void Awake()
    {
        eManager = FindObjectOfType<Enemy_Manager>();
        eManager.enemies.Add(this);
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
        if (willPatrol)
        {
            ChangeSuspicion(100);
            suspiciousArea = searchArea;

            state = Enemy_Manager.EnemyState.Search;
        }
    }
}
