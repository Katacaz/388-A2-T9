using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Node : MonoBehaviour
{
    public bool endOfPath;
    public List<Patrol_Node> connectedNodes = new List<Patrol_Node>();

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            try
            {
                Gizmos.DrawLine(this.transform.position, connectedNodes[i].transform.position);
            }
            catch (System.Exception)
            {
                Debug.LogWarning(name + " has not been connected to anything!");
                throw;
            }
            
        }
    }
}
