using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanGrabPlayer : MonoBehaviour
{
    public bool leftHand = false;
    public TitanBehavior behaviorScript;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            behaviorScript.GrabPlayer(leftHand);
        }
    }
}
