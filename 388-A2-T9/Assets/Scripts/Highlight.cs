using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Material defaultMat;
    public Material highlightMat;

    public SkinnedMeshRenderer mesh;

    public bool isHighlighted;
    // Start is called before the first frame update
    void Start()
    {
        defaultMat = mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartHighlight()
    {
        mesh.material = highlightMat;
        isHighlighted = true;
    }
    public void StopHighlight()
    {
        mesh.material = defaultMat;
        isHighlighted = false;
    }
}
