using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    public Rigidbody rb;
    public float atkStrength = 1000f;
    public float atkCooldown = 4f;
    public float maxAtkCharge = 1f;
    public float atkCharge;
    bool charging;
    public float atkTime = 1f;
    public bool doAttack;
    public bool onCooldown;

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
