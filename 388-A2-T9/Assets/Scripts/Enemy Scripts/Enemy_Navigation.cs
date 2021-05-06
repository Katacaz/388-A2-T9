using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class Enemy_Navigation : MonoBehaviour
{
    
    NavMeshAgent agent;
    Enemy enemyBase;
    Enemy_Manager.EnemyState state;

    [Header("Patrol Vars")]
    bool willPatrol;
    public Patrol_Node patrolPath;
    public float nodeDetectionRange = 2.0f;
    public float endOfPathTimer = 5.0f;
    private float endOfPathTime;
    public bool forwardPath;

    [Header("Search Vars")]
    public Vector3 searchPosition = new Vector3();
    public float searchRange = 2.0f;
    public float searchTimer = 5.0f;
    private float searchTime;

    [Header("Move Speeds")]
    public float patrolSpeed = 1.5f;
    public float searchSpeed = 2.5f;
    public float alertSpeed = 3.5f;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyBase = GetComponent<Enemy>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyBase.movingSpeed = agent.velocity.magnitude;
        state = enemyBase.state;
        willPatrol = enemyBase.willPatrol;
        searchPosition = enemyBase.suspiciousArea;
        switch (state)
        {
            case Enemy_Manager.EnemyState.Idle:
                Idle();
                break;
            case Enemy_Manager.EnemyState.Patrol:
                Patrol();
                break;
            case Enemy_Manager.EnemyState.Search:
                Search();
                break;
            case Enemy_Manager.EnemyState.Alert:
                Alert();
                break;
            case Enemy_Manager.EnemyState.Dead:
                Dead();
                break;
        }
    }

    public void Idle()
    {
        agent.isStopped = true;
    }
    public void Patrol()
    {
        agent.speed = patrolSpeed;
        if (patrolPath == null)
        {
            patrolPath = FindClosestPatrolPath();
        }
        float distanceToNode = Vector3.Distance(transform.position, patrolPath.transform.position);
        if (distanceToNode <= nodeDetectionRange)
        {
            //Gotten close enough to the node, change to next one
            if (patrolPath.endOfPath)
            {
                if (endOfPathTime < endOfPathTimer)
                {
                    endOfPathTime += Time.deltaTime;
                } else
                {
                    endOfPathTime = 0;
                    patrolPath = patrolPath.connectedNodes[0];
                    forwardPath = !forwardPath;
                }
            } else
            {
                if (forwardPath)
                {
                    patrolPath = patrolPath.connectedNodes[0];
                } else
                {
                    patrolPath = patrolPath.connectedNodes[1];
                }
            }
            agent.isStopped = true;

        }
        agent.isStopped = false;
        agent.SetDestination(patrolPath.transform.position);
        Debug.DrawLine(transform.position, patrolPath.transform.position, Color.green);

    }
    public void Search()
    {
        agent.speed = searchSpeed;
        float distanceToSearchPosition = Vector3.Distance(transform.position, searchPosition);
        if (distanceToSearchPosition <= searchRange)
        {
            //Gotten Close Enough to the search position, stay looking until suspicion drops
            if (enemyBase.suspicion > 0)
            {
                enemyBase.ChangeSuspicion(-(Time.deltaTime * 2f));
            }
            agent.isStopped = true;

        } else
        {
            agent.isStopped = false;
            agent.SetDestination(searchPosition);
            Debug.DrawLine(transform.position, searchPosition, Color.red);
        }
        //As they move to the position, the suspicion will drop on its own but slowly
        if (enemyBase.suspicion > 0)
        {
            enemyBase.ChangeSuspicion(-(Time.deltaTime));
        }
        else
        {
            if (searchTime < searchTimer)
            {
                searchTime += Time.deltaTime;
            }
            else
            {
                searchTime = 0;
                enemyBase.state = Enemy_Manager.EnemyState.Patrol;
                enemyBase.suspiciousArea = Vector3.zero;
            }

        }
    }
    public void Alert()
    {
        agent.speed = alertSpeed;
        float distanceToPlayer = Vector3.Distance(transform.position, enemyBase.playerObject.transform.position);
        if (distanceToPlayer <= 5.0f)
        {
            //Gotten close enough to player
        } else
        {
            agent.isStopped = false;
            agent.SetDestination(enemyBase.playerObject.transform.position);
        }
    }
    public void Dead()
    {
        if(agent.velocity.magnitude > 0)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.angularSpeed = 0;
            agent.speed = 0;
        }
    }
    private Patrol_Node FindClosestPatrolPath()
    {
        Patrol_Node[] allNodes = FindObjectsOfType<Patrol_Node>();
        int nodeID = 0;
        float nodeDistance = 10000;

        for (int i = 0; i < allNodes.Length; i++)
        {
            if (Vector3.Distance(transform.position, allNodes[i].transform.position) < nodeDistance)
            {
                nodeID = i;
                nodeDistance = Vector3.Distance(transform.position, allNodes[i].transform.position);
            }
        }
        return allNodes[nodeID];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(searchPosition, searchRange);
    }
}
