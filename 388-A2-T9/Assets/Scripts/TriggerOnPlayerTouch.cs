using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnPlayerTouch : MonoBehaviour
{
    public UnityEvent onEnter;
    [Tooltip("If '0' then trigger infinitely.")]
    public int maxTriggers;
    private int triggers;
    public void TriggeredByPlayer()
    {
        if (maxTriggers > 0)
        {
            //since a number was designated, there is a limit.
            if (triggers < maxTriggers)
            {
                onEnter.Invoke();
            }
        } else
        {
            //If 0 or less then it will trigger regardless.
            onEnter.Invoke();
        }       
        triggers++;
    }
}
