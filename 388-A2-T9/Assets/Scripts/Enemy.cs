using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Enemy_Manager eManager;

    public Animator enemyAnim;

    public bool dead;

    private void Awake()
    {
        eManager = FindObjectOfType<Enemy_Manager>();
        eManager.enemies.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyHit()
    {
        enemyAnim.SetTrigger("Hit");
    }
    public void EnemyDeath()
    {
        dead = true;
        enemyAnim.SetTrigger("Death");
        //eManager.enemies.Remove(this);
        if (!eManager.defeatedEnemies.Contains(this))
        {
            eManager.defeatedEnemies.Add(this);
        }
    }
}
