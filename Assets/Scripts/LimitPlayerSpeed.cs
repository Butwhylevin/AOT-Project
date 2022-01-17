using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class LimitPlayerSpeed : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 10f;
    public Rigidbody rb;
    public vThirdPersonMotor motorScript;
    public float slideSpeed = 30f;

    void Update()
    {
        speed = rb.velocity.magnitude;

        if(speed > maxSpeed){
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        //you can slide on the ground if you're going fast enough, makes movement more fluid
        //if(speed > slideSpeed)
        //{
         //   motorScript.slide = true;
        //}
        //else
        //{
         //   motorScript.slide = false;
        //}
    }
}
