using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanBehavior : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        DisableRagdoll();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            EnableRagdoll();
        }
    }

    private void DisableRagdoll()
    {
        anim.enabled = true;
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
        anim.enabled = false;
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }

}
