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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (dTimer < deathTime)
        {
            dTimer += Time.deltaTime;
        } else
        {
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
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Arrow Collided with " + other.transform.name);
        //Check if 
        Arrow_Target tar = other.GetComponent<Arrow_Target>();
        if (tar != null)
        {
            //Target hit
            tar.TargetHit(damageAmount, this.GetComponent<SphereCollider>().center);
            DestroyArrow();
        }
        if (other.gameObject.layer == hitLayer)
        {
            DestroyArrow();
        }
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
