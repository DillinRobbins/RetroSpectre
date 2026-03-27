using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private int contactCount = 0;

    private void OnTriggerEnter(Collider collider)
    {
        if(!collider.isTrigger && !gameObject.CompareTag("Player"))
        {
            contactCount++;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(!collider.isTrigger && !gameObject.CompareTag("Player"))
        {
            contactCount--;
        }
    }

    public bool IsGrounded()
    {
        return contactCount > 0;
    }
}
