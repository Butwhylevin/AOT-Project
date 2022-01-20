using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapeBehavior : MonoBehaviour
{
    public TitanBehavior behaviorScript;
    public void KillTitan(Vector3 killPoint)
    {
        behaviorScript.EnableRagdoll(); // add force from the player loc later
    }
}
