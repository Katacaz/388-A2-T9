using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arrow_Target : MonoBehaviour
{
    [Tooltip("Object that will be destroyed, if empty it destroys this object")]
    public GameObject parent;
    [Header("Stats")]
    public int health;
    [Header("Effects")]
    [Tooltip("Effect Prefab made if hit and survives")]
    public GameObject hitEffect;
    [Tooltip("Effect Prefab made if destroyed")]
    public GameObject destroyEffect;

    public UnityEvent onHit;
    public UnityEvent onDeath;
    [Tooltip("Check for if this destroys object or relies on something else.")]
    public bool thisDestroysObject = true;

    public Vector3 hitFromDirection = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TargetHit(int damageAmount, Vector3 hitPosition)
    {
        hitFromDirection = hitPosition - this.transform.position;
        if (health > 0)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                //Target Dead
                TargetDestroyed();
            }
            else
            {
                //Target still alive
                onHit.Invoke();
                if (hitEffect != null)
                {
                    GameObject effect = Instantiate(hitEffect);
                    effect.transform.position = hitPosition;
                    Destroy(effect, 2.0f);
                }
            }
        }
    }

    public void TargetDestroyed()
    {
        onDeath.Invoke();
        if (destroyEffect != null)
        {
            GameObject effect = Instantiate(destroyEffect);
            effect.transform.position = this.transform.position;
            Destroy(effect, 2.0f);
        }
        if (thisDestroysObject)
        {
            if (parent != null)
            {
                Destroy(parent);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
