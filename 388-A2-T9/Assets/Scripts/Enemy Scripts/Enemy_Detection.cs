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

    bool searching;
    bool alert;

    private void Awake()
    {
        enemyBase = GetComponent<Enemy>();
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

        searchImage.SetActive(searching);
        alertImage.SetActive(alert);
        searchFill.fillAmount = (enemyBase.suspicion / 100f);

    }
}
