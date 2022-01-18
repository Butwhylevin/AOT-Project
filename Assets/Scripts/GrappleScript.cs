using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using UnityEngine.UI;

public class GrappleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer rend1, rend2;
    public Transform grapplePoint1, grapplePoint2;
    bool movingGrapple1, movingGrapple2;
    public LayerMask whatIsGrappleable;
    public Transform cameraPoint, player; 
    public Transform gunPoint1, gunPoint2;
    public float grappleRayOffset;
    SpringJoint joint1, joint2; 
    public float maxDist = 100;
    public float spring=100f, damper=0.1f, massScale=1f, maxDistance=0.8f, minDistance=0.25f;
    public bool grappling;
    Rigidbody rb;
    Vector3 rayPosLeft, rayPosRight;

    //pull
    public float pullSpring = 100f, pullMassScale = 4f;

    //gas
    public float gasStrength = 100f;
    public float gasAmount, maxGasAmount=10f;
    public float gasWait, maxGasWait = 200f; 

    //splitting
    public float splitIncreaseInterval = 2f;
    public float maxSplitHitDist = 100f;
    public float maxSplitDiff = 50f;
    public float maxSplitAmount = 30f;
    public Vector3 playerForward;
    public GameObject debugSphere1, debugSphere2;

    //splitting ui
    public Image leftUI, rightUI;

    bool split = false;

    public ParticleSystem smokePart;

    //sliding
    public vThirdPersonMotor movementScript;
    public LimitPlayerSpeed speedScript;
    public float slideSpeed = 10f;

    void Start()
    {
        rend1.positionCount = 0;
        rend2.positionCount = 0;

        gasAmount = maxGasAmount;
        gasWait = maxGasWait;

        rb = player.gameObject.GetComponent<Rigidbody>();
        smokePart.Stop();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGrapple(-1,1);
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopGrapple(1);
        }

        if(Input.GetMouseButtonDown(1))
        {
            StartGrapple(1,2);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            StopGrapple(2);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            DoSplit();
        }
        else
        {
            split = false;
        }

        if(joint1 != null || joint2 != null)
        {
            grappling = true;
        }
        else
        {
            grappling = false;
        }
        
        CheckForPull();

        MovingGrapplePoints();

        if(grappling && speedScript.speed > slideSpeed)
        {
            movementScript.sliding = true;
        }
        else
        {
            movementScript.sliding = false;
        }

        playerForward = transform.forward;

        //debug
        debugSphere1.transform.position = rayPosLeft;
        debugSphere2.transform.position = rayPosRight;
    }

    void FixedUpdate()
    {
        if(gasAmount > 0)
        {
            if(Input.GetKey(KeyCode.E))
            { 
                DoGas();
            }
        }
        else
        {
            if(smokePart.isEmitting)
            {
                smokePart.Stop();
            }
        }

        if(gasAmount<maxGasAmount)
        {
            gasWait--;
            if(gasWait == 0)
            {
                gasWait = maxGasWait;
                gasAmount = maxGasAmount;
            }
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void MovingGrapplePoints()
    {
        if(grapplePoint1 != null && movingGrapple1)
        {
            // update the joint
            joint1.connectedAnchor = grapplePoint1.position;
        }
        if(grapplePoint2 != null && movingGrapple2)
        {
            // update the joint
            joint2.connectedAnchor = grapplePoint2.position;
        }
    }

    void StartGrapple(float offsetMultiplier, float grappleNumb)
    {
        RaycastHit hit;
        Vector3 rayPos = Vector3.zero;
        if(split)
        {
            if(grappleNumb == 1)
            {
                rayPos = rayPosLeft;
            }
            if(grappleNumb == 2)
            {
                rayPos = rayPosRight;
            }
        }
        else
        {
            rayPos = new Vector3(cameraPoint.position.x + grappleRayOffset * offsetMultiplier,cameraPoint.position.y,cameraPoint.position.z);
        }

        if(Physics.Raycast(origin: rayPos, direction: cameraPoint.forward, out hit, maxDist, layerMask: whatIsGrappleable))
        {
            if(grappleNumb == 1)
            {
                // if the object doesn't have a rigibody, then just stick to it, otherwise, I need to be updating it
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    // make the grapple point a child of hit object
                    grapplePoint1.position = hit.point;
                    grapplePoint1.transform.parent = hit.transform;
                    movingGrapple1 = true;
                }
                else
                {
                    grapplePoint1.position = hit.point;
                    movingGrapple1 = false;
                }

                joint1 = player.gameObject.AddComponent<SpringJoint>();
                joint1.autoConfigureConnectedAnchor = false;

                joint1.connectedAnchor = grapplePoint1.position;

                float distFromPoint = Vector3.Distance(player.position, grapplePoint1.position);
                joint1.maxDistance = distFromPoint * maxDistance;
                joint1.minDistance = distFromPoint * minDistance;
                rend1.positionCount = 2;

                joint1.spring = spring;
                joint1.damper = damper;
                joint1.massScale = massScale;
            }
            if(grappleNumb == 2)
            {
                // if the object doesn't have a rigibody, then just stick to it, otherwise, I need to be updating it
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    // make the grapple point a child of hit object
                    grapplePoint2.position = hit.point;
                    grapplePoint2.transform.parent = hit.transform;
                    movingGrapple2 = true;
                }
                else
                {
                    grapplePoint2.position = hit.point;
                    movingGrapple2 = false;
                }

                joint2 = player.gameObject.AddComponent<SpringJoint>();
                joint2.autoConfigureConnectedAnchor = false;

                joint2.connectedAnchor = grapplePoint2.position;

                float distFromPoint = Vector3.Distance(player.position, grapplePoint2.position);
                joint2.maxDistance = distFromPoint * maxDistance;
                joint2.minDistance = distFromPoint * minDistance;
                rend2.positionCount = 2;

                joint2.spring = spring;
                joint2.damper = damper;
                joint2.massScale = massScale;
            }
        }
    }

    void StopGrapple(float grappleNumb)
    {
        if(grappleNumb == 1)
        {
            rend1.positionCount = 0;
            Destroy(joint1);
            if(movingGrapple1)
            {
                movingGrapple1 = false;
                grapplePoint1.transform.parent = null;
            }
        }
        if(grappleNumb == 2)
        {
            rend2.positionCount = 0;
            Destroy(joint2);
            if(movingGrapple2)
            {
                movingGrapple2 = false;
                grapplePoint2.transform.parent = null;
            }
        }
    }

    void DoGas()
    {
        if(gasAmount > 0)
        {
            if(!smokePart.isEmitting)
            {
                smokePart.Play();
            }
            rb.AddForce(Camera.main.transform.forward * gasStrength);
            gasAmount--;
        }
    }

    void DrawRope()
    {
        if(grapplePoint1 != null)
        {
            rend1.SetPosition(0, gunPoint1.position);
            rend1.SetPosition(1, grapplePoint1.position);
        }
        if(grapplePoint2 != null)
        {
            rend2.SetPosition(0, gunPoint2.position);
            rend2.SetPosition(1, grapplePoint2.position);
        }
    }

    void DoSplit()
    {
        bool foundLeft = false;
        float movedAmountL = 0;
        split = true;
        while(!foundLeft)
        {
            //raycast forward to see if you can grapple from this point
            RaycastHit hit;
            
            rayPosLeft = new Vector3(cameraPoint.position.x - movedAmountL,cameraPoint.position.y,cameraPoint.position.z);
            if(!Physics.Raycast(origin: rayPosLeft, direction: cameraPoint.forward, out hit, maxDist, layerMask: whatIsGrappleable))
            {
                //if not then move further to the side and keep trying
                movedAmountL += splitIncreaseInterval;
            }
            else
            {
                //if yes then move on
                foundLeft = true;
            }
            if(!foundLeft && (movedAmountL >= maxSplitAmount))
            {
                //if reached max amount, then end function
                foundLeft = true;
            }
        }

        bool foundRight = false;
        float movedAmountR = 0;
        while(!foundRight)
        {
            //raycast forward to see if you can grapple from this point
            RaycastHit hit;
            
            rayPosRight = new Vector3(cameraPoint.position.x + movedAmountR,cameraPoint.position.y,cameraPoint.position.z);
            if(!Physics.Raycast(origin: rayPosRight, direction: cameraPoint.forward, out hit, maxDist, layerMask: whatIsGrappleable))
            {
                //if not then move further to the side and keep trying
                movedAmountR += splitIncreaseInterval;
            }
            else
            {
                //if yes then move on
                foundRight = true;
            }
            if(!foundRight && (movedAmountR >= maxSplitAmount))
            {
                //if reached max amount, then end function
                foundRight = true;
            }
        }

    }

    void CheckForPull()
    {
        if(joint1 != null)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                //gas
                joint1.spring = pullSpring;
                joint1.massScale = pullMassScale;
            }
            else
            {
                joint1.spring = spring;
                joint1.massScale = massScale;

            }
        }
        if(joint2 != null)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                //gas
                joint2.spring = pullSpring;
                joint2.massScale = pullMassScale;
            }
            else 
            {
                joint2.spring = spring;
                joint2.massScale = massScale;

            }
        }
    }
}
