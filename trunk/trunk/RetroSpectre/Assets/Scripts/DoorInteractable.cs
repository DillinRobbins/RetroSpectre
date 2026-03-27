using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private Material[] HighlightedMaterials = new Material[2];
    private Material[] UnhighlightedMaterials = new Material[2];
    public Material Outline;
    public Animator DoorAnimator;

    private void Awake()
    {
        HighlightedMaterials[0] = GetComponent<MeshRenderer>().material;
        HighlightedMaterials[1] = Outline;
        UnhighlightedMaterials[0] = GetComponent<MeshRenderer>().material;
        UnhighlightedMaterials[1] = GetComponent<MeshRenderer>().material;
    }

    public void Interact()
    {
        bool doorOpen = DoorAnimator.GetBool("DoorOpen");

        if (doorOpen)
        {
            DoorAnimator.SetBool("DoorOpen", false);
        }
        else
        {
            DoorAnimator.SetBool("DoorOpen", true);
        }
    }

    public void Highlight()
    {
        gameObject.GetComponent<MeshRenderer>().materials = HighlightedMaterials;
    }

    public void Unhighlight()
    {
        gameObject.GetComponent<MeshRenderer>().materials = UnhighlightedMaterials;
    }
}
