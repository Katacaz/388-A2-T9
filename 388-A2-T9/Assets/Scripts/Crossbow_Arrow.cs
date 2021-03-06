using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow_Arrow : MonoBehaviour
{
    public float arrowSpeed = 2.0f;

    public float deathTime = 5.0f;
    private float dTimer;
    public GameObject destroyEffect;

    public int damageAmount = 1;

    public Arrow_Target target;

    public LayerMask hitLayer;

    Vector3 prev = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dTimer < deathTime)
        {
            dTimer += Time.deltaTime;
        } else
        {
            DestroyArrow();
        }
    }
    private void FixedUpdate()
    {
        Move();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, arrowSpeed * Time.deltaTime, hitLayer, QueryTriggerInteraction.Collide))
        {
            
            Arrow_Target tar = hit.collider.GetComponent<Arrow_Target>();
            if (tar != null)
            {
                tar.TargetHit(damageAmount, hit.point);
                
            }
            DestroyArrow();
        }
    }
    public void SetTarget(Arrow_Target tar)
    {
        target = tar;
        if (target != null)
        {
            transform.LookAt(target.transform.position);
        }
    }
    public void Move()
    {
        transform.position += this.transform.forward * arrowSpeed * Time.deltaTime;
    }

    public void DestroyArrow()
    {
        if (destroyEffect != null)
        {
            GameObject effect = Instantiate(destroyEffect);
            effect.transform.position = this.transform.position;
            Destroy(effect, 2.0f);
        }
        Destroy(this.gameObject);
    }
}
