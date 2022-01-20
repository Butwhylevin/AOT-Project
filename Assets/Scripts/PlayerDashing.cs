using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    public Transform cam;
    public float dCTime = 0.3f;
    public float dashCooldown = 2f;
    public string lastClicked;
    public Rigidbody rb;
    public float dashForce;
    bool canDash = true;

    private void Update() 
    {
        if(canDash)
        {
            DashInput();
        }
    } 

    private void DashInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            // WASD
            if(lastClicked != "w")
            {
                lastClicked = "w";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(cam.forward);
            }
        }

        if(Input.GetKeyDown(KeyCode.D))
        {

            if(lastClicked != "d")
            {
                lastClicked = "d";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(cam.right);
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {

            if(lastClicked != "a")
            {
                lastClicked = "a";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(-cam.right);
            }
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(lastClicked != "s")
            {
                lastClicked = "s";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(-cam.forward);
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            // UP / DOWN
            if(lastClicked != "e")
            {
                lastClicked = "e";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(transform.up);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(lastClicked != "q")
            {
                lastClicked = "q";
                Invoke("RemoveLastClicked", dCTime);
            }
            else
            {
                DoDash(-transform.up);
            }
        }
    }

    private void RemoveLastClicked()
    {
        lastClicked = null;
    }

    private void DoDash(Vector3 dir)
    {
        RemoveLastClicked();

        // set velocity to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // dash in chosen direction
        rb.AddForce(dir * dashForce);

        canDash = false;
        Invoke("DashCooldown", dashCooldown);
    }

    private void DashCooldown()
    {
        canDash = true;
    }
}
