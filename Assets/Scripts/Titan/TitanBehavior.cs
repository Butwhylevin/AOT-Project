﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Random = UnityEngine.Random;

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
    public Rigidbody hipsRb;
    public float hipsForce = 5000f;

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
    public Transform attackMover;
    public float turnForRepeats = 10f;
    public float slowRotSpeed;

    [Header("Camera Shake")]
    public float magn;
    public float rough, fadeIn, fadeOut;

    [Header("Eat Player")]
    public GameObject playerObj;
    public GameObject playerCam, eatPlayerModelr, eatPlayerCamerar;
    public GameObject bloodPrefab;
    public bool eatSound = false;

    [Header("Sounds")]
    public AudioSource audio;
    public AudioClip dieClip, eatClip, stompClip;

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
            DoEatPlayer();
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
            ikScript.doAttack = true;
            DoAttacking();
        }
    }

    private void DoAttacking()
    {
        attackMover.LookAt(target.position);
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
            audio.pitch = Random.Range(0.8f,1.2f);
            audio.PlayOneShot(stompClip, 1f);
            audio.pitch = 1f;
        }
        if(lFootPart && curPartWaitTime < 0)
        {
            lFootPart = false;
            CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
            Instantiate(footPartPrefab, lFoot.position, lFoot.rotation);
            curPartWaitTime = waitTime;
            audio.pitch = Random.Range(0.8f,1.2f);
            audio.PlayOneShot(stompClip, 1f);
            audio.pitch = 1f;
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

    public void TurnToFacePlayer()
    {
        StartCoroutine("TurnForSeconds");
    }

    IEnumerator TurnForSeconds()
    {
        int i = 0;
        while(i < turnForRepeats)
        {
            Vector3 D = target.position - transform.position;  
            nextStepRot = Quaternion.Slerp(nextStepRot, Quaternion.LookRotation(D), slowRotSpeed * Time.deltaTime);
            UpdateRot();
            i ++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EnableRagdoll()
    {
        dead = true;
        audio.PlayOneShot(dieClip, 1f);
        anim.enabled = false;
        Instantiate(bloodPrefab, target.position, target.rotation);
        foreach(Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddForce(rb.gameObject.transform.forward * hipsForce);
        }
        //hipsRb.AddForce(hipsRb.gameObject.transform.forward * hipsForce);
    }

    public void GrabPlayer(bool leftHand)
    {
        if(!dead)
        {
            ikScript.ikActive = false;
            playerObj.SetActive(false);
            playerCam.SetActive(false);
            if(!leftHand)
            {
                eatPlayerModelr.SetActive(true);
                eatPlayerCamerar.SetActive(true);
            }

            anim.SetBool("isEating", true);
        }
    }

    private void DoEatPlayer()
    {
        if(eatSound)
        {
            eatSound = false;
            audio.PlayOneShot(eatClip, 1f);
            Instantiate(bloodPrefab, eatPlayerModelr.transform.position, eatPlayerModelr.transform.rotation);
        }
    }

}
