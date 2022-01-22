using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class TitanBehavior : MonoBehaviour
{
    [Header("Movement")]
    public Transform target;
    public float moveSpd;
    public float rotSpeed = 1f;
    public float minDist = 25f, maxDist = 30f;
    public float curDist;
    bool moving;
    bool crouching;

    [Header("Feet Particles")]
    public Transform rFoot;
    public Transform lFoot;
    public bool rFootPart, lFootPart;
    public GameObject footPartPrefab;
    public float waitTime = 0.5f;
    float curPartWaitTime;
    
    Quaternion nextStepRot;
    bool dead;
    Animator anim;
    [Header("Arm IK")]
    public TitanArmIK ikScript;

    [Header("Camera Shake")]
    public float magn;
    public float rough, fadeIn, fadeOut;

    [Header("Eat Player")]
    public GameObject playerObj;
    public GameObject playerCam, eatPlayerModelr;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        DisableRagdoll();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            EnableRagdoll();
        }

        if(!dead)
        {
            CheckDistanceFromPlayer();
            Animation();
            if(moving)
            {
                Movement();
                DoFeetParticles();
                UpdateRot();
            }
        }
    }

    private void Animation()
    {
        anim.SetBool("isMoving", moving);
    }

    private void Movement()
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

    private void CheckDistanceFromPlayer()
    {
        // checks the distance from the player, not taking into account the y
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.y = 0;
        curDist = vectorToTarget.magnitude;

        if(moving)
        {
            moving = (curDist > minDist);
            ikScript.doAttack = false;
        }
        else
        {
            moving = (curDist > maxDist);
            DoCrouch();
        }
    }

    private void DoCrouch()
    {
        if(target.position.y < 15)
        {
            crouching = true;
            // activate arm IK
            ikScript.doAttack = true;
        }
        else
        {
            crouching = false;
            // deactivate arm IK
            ikScript.doAttack = false;
        }
        anim.SetBool("playerBelow", crouching);
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

    public void UpdateRot()
    {
        transform.rotation = nextStepRot;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void DisableRagdoll()
    {
        anim.enabled = true;
        foreach(Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        dead = true;
        anim.enabled = false;
        foreach(Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }

    public void GrabPlayer(bool leftHand)
    {
        playerObj.SetActive(false);
        playerCam.SetActive(false);
        if(!leftHand)
        {
            eatPlayerModelr.SetActive(true);
        }

        anim.SetBool("isEating", true);
    }

}
