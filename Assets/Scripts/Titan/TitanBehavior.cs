using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class TitanBehavior : MonoBehaviour
{
    public Transform target;
    public float moveSpd;
    public float rotSpeed = 1f;
    public Transform rFoot, lFoot;
    public bool rFootPart, lFootPart;
    public GameObject footPartPrefab;
    public float waitTime = 0.5f;
    float curPartWaitTime;
    Quaternion nextStepRot;
    bool dead;
    Animator anim;
    [Header("Camaera Shake")]
    public float magn;
    public float rough, fadeIn, fadeOut;

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

            DoFeetParticles();
        }
        
    }

    private void DoFeetParticles()
    {
        curPartWaitTime -= Time.deltaTime;
        if(rFootPart && curPartWaitTime < 0)
        {
            rFootPart = false;
            CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
            Instantiate(footPartPrefab, rFoot.position, rFoot.rotation);
            curPartWaitTime = waitTime;
        }
        if(lFootPart && curPartWaitTime < 0)
        {
            lFootPart = false;
            CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
            Instantiate(footPartPrefab, lFoot.position, lFoot.rotation);
            curPartWaitTime = waitTime;
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
