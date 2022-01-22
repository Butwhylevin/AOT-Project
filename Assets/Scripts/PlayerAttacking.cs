using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerAttacking : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public GameObject attackModel, moveModel, moveRig, moveThing1, moveThing2, moveGrapple;
    public float atkStrength = 1000f;
    public float atkCooldown = 4f;
    public float maxAtkCharge = 1f;
    public float atkCharge;
    bool charging;
    public float atkTime = 1f;
    public bool doAttack;
    public bool onCooldown;

    [Header("Camera Shake")]
    public float magn;
    public float rough, fadeIn, fadeOut;

    public AudioSource audio;
    public AudioClip attackClip;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && !onCooldown)
        {
            charging = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            charging = false;
            atkCharge = 0;
        }

        if(charging)
        {
            DoCharging();
        }

        anim.SetBool("doAttack", doAttack);
        DoAnimation();
    }

    private void DoAnimation()
    {
        if(charging || doAttack)
        {
            attackModel.SetActive(true);
            moveModel.SetActive(false);
            moveRig.SetActive(false);
            moveThing1.SetActive(false);
            moveThing2.SetActive(false);
            moveGrapple.SetActive(false);
        }
        else
        {
            attackModel.SetActive(false);
            moveModel.SetActive(true);
            moveRig.SetActive(true);
            moveThing1.SetActive(true);
            moveThing2.SetActive(true);
            moveGrapple.SetActive(true);
        }
        
    }

    private void DoCharging()
    {
        atkCharge += Time.deltaTime;
        if(atkCharge >= maxAtkCharge)
        {
            charging = false;
            DoAttack();
        }
    }

    private void DoAttack()
    {
        // reset charge
        atkCharge = 0;
        // screenshake
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
        // sound
        audio.PlayOneShot(attackClip, 1f);
        // apply forces
        rb.AddForce(Camera.main.transform.forward * atkStrength);
        // attacking
        doAttack = true;
        Invoke("EndAttack", atkTime);
        // cooldown
        onCooldown = true;
        Invoke("EndCooldown", atkCooldown);
    }

    private void EndCooldown()
    {
        onCooldown = false;
    }

    private void EndAttack()
    {
        doAttack = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(doAttack && other.gameObject.tag == "titanNape")
        {
            Debug.Log("HIT NAPE");
            other.gameObject.GetComponent<NapeBehavior>().KillTitan(transform.position);
        }
    }

}
