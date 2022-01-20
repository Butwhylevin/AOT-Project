using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public Transform target;
    public Transform body;
    public TitanBehavior behaviorScript;
    public LayerMask canWalk;
    public float stepSpeed = 5f;
    public float lerpSpeed = 1f;
    public bool doStepOffset;
    public float minDist = 0.5f;
    public AnimationCurve stepCurve;
    public float maxStepHeight = 2f;
    bool doLerp;
    float curStep;
    float startLerpTime;
    float lerpDistance;

    private void Start() 
    {
        if(doStepOffset)
        {
            curStep += stepSpeed * 0.5f;
        }
    }

    private void Update() 
    {
        curStep += Time.deltaTime;

        if(curStep > stepSpeed)
        {
            doLerp = true;
            curStep = 0;
            startLerpTime = Time.time;
            lerpDistance = Vector3.Distance(transform.position, target.position);
        }

        if(doLerp)
        {
            float distCovered = (Time.time - startLerpTime) * lerpSpeed;
            float fractionOfLerp = distCovered / lerpDistance;

            float curDist = Vector3.Distance(transform.position, target.position);
            if(curDist <= minDist)
            {
                doLerp = false;
            }

            // curve that increases the height of the step
            float increaseHeight = stepCurve.Evaluate(curDist / lerpDistance) * maxStepHeight;
            transform.GetChild(0).localPosition = new Vector3(0,0,increaseHeight);
            
            transform.position = Vector3.Lerp(transform.position, target.position, fractionOfLerp);
            
            //behaviorScript.UpdateRot(fractionOfLerp);
        }
    }
}
