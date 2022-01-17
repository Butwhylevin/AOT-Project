using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    public GrappleScript grappleScript;
    Vector3 facePoint;

    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && grappleScript.grappling)
        {
            //face towards grapple point 1 or 2
            if(grappleScript.grapplePoint1 != null)
            {
                facePoint = grappleScript.grapplePoint1;
            }
            else
            {
                facePoint = grappleScript.grapplePoint2;
            }

            transform.LookAt(facePoint);

        }
    }

}
