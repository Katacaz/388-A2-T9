using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Rotation:" + transform.localEulerAngles.x + " " + transform.localEulerAngles.y + " " + transform.localEulerAngles.z);
    }
}
