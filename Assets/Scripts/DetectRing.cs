using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRing : MonoBehaviour
{
    public RingControl ringScript;
    public bool firstRing;
    public bool lastRing;
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(firstRing)
            {
                ringScript.StartFirstRing();
            }
            else if(lastRing)
            {
                ringScript.FinishLastRing();
            }
            else
            {
                ringScript.GoToNextRing();
            }
        }
    }
}
