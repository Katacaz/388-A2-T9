using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Point : MonoBehaviour
{

    public List<Teleport_Point> connectedPoints = new List<Teleport_Point>();

    public GameObject linePrefab;
    private List<LineRenderer> lines = new List<LineRenderer>();
    public bool linePreviewActive;

    public float range = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        SetUpLines();
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedPoints.Count > 0)
        {
            foreach (LineRenderer l in lines)
            {
                if (l != null)
                {
                    l.gameObject.SetActive(linePreviewActive);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            linePreviewActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            linePreviewActive = false;
        }
    }
    public void SetUpLines()
    {
        if (connectedPoints.Count > 0)
        {
            for (int i = 0; i < connectedPoints.Count; i++)
            {
                LineRenderer line = Instantiate(linePrefab, this.transform).GetComponent<LineRenderer>();
                line.SetPosition(0, this.transform.position);
                line.SetPosition(1, connectedPoints[i].transform.position);
                lines.Add(line);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }
}
