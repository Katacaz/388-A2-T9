using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Outline))]
public class Highlight : MonoBehaviour
{
    //private Material defaultMat;
    //public Material highlightMat;

    public SkinnedMeshRenderer mesh;
    public MeshRenderer mR;

    public bool isHighlighted;

    public GameObject highlightObject;
    private Outline outline;
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (mesh != null)
        {
            //defaultMat = mesh.material;
        }
        else if (mR != null)
        {
            //defaultMat = mR.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        outline.enabled = isHighlighted;
        if (highlightObject != null)
        {
            highlightObject.SetActive(isHighlighted);
        }
    }
    public void StartHighlight()
    {
        //ChangeMaterial(highlightMat);
        isHighlighted = true;
    }
    public void StopHighlight()
    {
        //ChangeMaterial(defaultMat);
        isHighlighted = false;
    }
    public void ChangeMaterial(Material mat)
    {
        if (mesh != null)
        {
            mesh.material = mat;
        } else if (mR != null)
        {
            mR.material = mat;
        }
    }
}
