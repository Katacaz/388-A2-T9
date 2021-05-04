using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Alerter : MonoBehaviour
{

    public float alertRange = 25.0f;
    public float suspicionAmount = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlertEnemies()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < alertRange)
            {
                allEnemies[i].StartSearch(this.transform.position, suspicionAmount);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}
