using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell_Object : MonoBehaviour
{
    public Animator bellAnim;
    public Arrow_Target target;
    public void BellHit()
    {
        Quaternion lookRot = Quaternion.LookRotation(target.hitFromDirection, Vector3.up);
        lookRot.x = 0;
        lookRot.z = 0;
        transform.rotation = lookRot;
        bellAnim.SetTrigger("Hit");
    }
}
