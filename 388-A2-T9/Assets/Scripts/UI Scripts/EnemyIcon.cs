using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    public Enemy enemyInfo;
    private bool enemyDead;
    [Header("UI Elements")]
    public Image enemyIcon;
    public Image enemyDeadIcon;
    [Header("Icons")]
    public Sprite crossIcon;

    public void SetUpIcon(Enemy enemy)
    {
        enemyInfo = enemy;
        enemyIcon.sprite = enemyInfo.icon;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInfo != null)
        {
            enemyDead = enemyInfo.dead;
            enemyDeadIcon.gameObject.SetActive(enemyDead);
        }
    }
}
