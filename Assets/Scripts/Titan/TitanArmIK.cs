using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class TitanArmIK : MonoBehaviour {
    
    protected Animator animator;
    public TitanBehavior behaviorScript;
    
    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;
    public Transform rHandBone;
    public bool doAttack = false;
    float curStep = 0;

    void Start () 
    {
        animator = GetComponent<Animator>();
    }
    
    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(animator) {
            
            //if the IK is active, set the position and rotation directly to the goal. 
            if(ikActive) {

                /// LOOK IK

                // Set the look target position, if one has been assigned
                if(lookObj != null) 
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }    

                /// ARM IK

                // Set the right hand target position and rotation, if one has been assigned
                if(doAttack)
                {
                    if(rightHandObj != null) 
                    {
                        float dotProduct = Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(rightHandObj.position));
                        Debug.Log(dotProduct);
                        if(dotProduct > 0)
                        {
                            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                            animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
                            animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandObj.position);
                            Quaternion rot = Quaternion.LookRotation(rightHandObj.position - rHandBone.position);
                            animator.SetIKRotation(AvatarIKGoal.RightHand,rot);
                        }
                        else
                        {
                            behaviorScript.TurnToFacePlayer();
                        }
                    }      
                }                  
            }
            
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {          
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
                animator.SetLookAtWeight(0);
            }
        }
    }    
}