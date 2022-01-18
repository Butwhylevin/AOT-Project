using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour {
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public GrappleScript grapplingGun;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    public float grappleNumb = 1;
    Vector3 gunTipPosition;
    
    void Awake() {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }
    
    //Called after Update
    void LateUpdate() {
        if(grappleNumb == 1)
        {
            gunTipPosition = grapplingGun.gunPoint1.position;
        }
        if(grappleNumb == 2)
        {
            gunTipPosition = grapplingGun.gunPoint2.position;
        }
        DrawRope();
    }

    void DrawRope() {
        //If not grappling, don't draw rope
        if (!grapplingGun.grappling) {
            currentGrapplePosition = gunTipPosition;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0) {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
        
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = grapplingGun.grapplePoint1;
        if(grappleNumb == 2)
        {
            grapplePoint = grapplingGun.grapplePoint2;
        }

        if(grappleNumb == 1)
        {
            gunTipPosition = grapplingGun.gunPoint1.position;
        }
        if(grappleNumb == 2)
        {
            gunTipPosition = grapplingGun.gunPoint2.position;
        }

        var up = Quaternion.LookRotation((grapplePoint.position - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint.position, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++) {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);
            
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}