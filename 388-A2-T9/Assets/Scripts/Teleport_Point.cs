using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Point : MonoBehaviour
{
    private GameObject player;

    public List<Teleport_Point> connectedPoints = new List<Teleport_Point>();

    public GameObject linePrefab;
    private List<LineRenderer> lines = new List<LineRenderer>();
    public bool linePreviewActive;

    public GameObject particleFX;
    public bool hideParticlesOutOfRange = true;

    private float teleportRange;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        SetUpLines();
    }

    // Update is called once per frame
    void Update()
    {
        teleportRange = player.GetComponent<Player>().tpTool.aimDistance;
        UpdateParticleEnabled();
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
    public void UpdateParticleEnabled()
    {
        if (hideParticlesOutOfRange)
        {
            if (PlayerDistanceFromPoint() <= teleportRange)
            {
                //Player in range of teleporter.
                particleFX.SetActive(true);
            } else
            {
                //Player not in range.
                particleFX.SetActive(false);
            }
        }
    }
    public float PlayerDistanceFromPoint()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
}
