using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Outline))]
public class Highlight : MonoBehaviour
{
    public bool isHighlighted;

    public GameObject highlightObject;
    private Outline outline;
    [Tooltip("Disable this if the highlight just needs to enable/disable objects and not actually highlight")]
    public bool useOutline = true;
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (useOutline)
        {
            outline.enabled = isHighlighted;
        } else
        {
            outline.enabled = false;
        }
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
}
