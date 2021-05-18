using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Arrow_Target))]
public class Enemy_Alerter : MonoBehaviour
{
    Arrow_Target targetInfo;
    public float alertRange = 25.0f;
    public float suspicionAmount = 100f;

    public float pointToOriginRange = 2f;
    private void Awake()
    {
        targetInfo = GetComponent<Arrow_Target>();
    }
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
            if (Vector3.Distance(transform.position, allEnemies[i].transform.position) < alertRange &&
                Vector3.Distance(transform.position, allEnemies[i].transform.position) > pointToOriginRange)
            {
                //If the enemy is beyond the Point To Origin Range but within range of being alerted, bring them to this objects position.
                allEnemies[i].StartSearch(this.transform.position, suspicionAmount);
            } else if (Vector3.Distance(transform.position, allEnemies[i].transform.position) <= pointToOriginRange)
            {
                //If the enemy is close enough to be in the Point To Origin Range, instead point them in the direction this was hit.
                Debug.Log(allEnemies[i].transform.name + " saw where the arrow came from.");
                allEnemies[i].StartSearch(this.transform.position + targetInfo.hitFromDirection * 10f, suspicionAmount);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pointToOriginRange);
    }
}
