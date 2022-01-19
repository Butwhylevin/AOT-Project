using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanBehavior : MonoBehaviour
{
    public Transform target;
    public float moveSpd;
    public float rotSpeed = 1f;
    Quaternion nextStepRot;
    bool dead;
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

        if(!dead)
        {
            /// ROTATE TOWARDS PLAYER
            // distance between target and the actual rotating object
            Vector3 D = target.position - transform.position;  
            
            // calculate the Quaternion for the rotation
            Quaternion rot = Quaternion.Slerp(nextStepRot, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);

            //Apply the rotation 
            nextStepRot = rot; 

            // move forwards
            transform.position += transform.forward * Time.deltaTime * moveSpd;
        }
        
    }

    public void UpdateRot(float frac)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, nextStepRot, frac);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void DisableRagdoll()
    {
        anim.enabled = true;
        foreach(Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }

    private void EnableRagdoll()
    {
        dead = true;
        anim.enabled = false;
        foreach(Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }

}
