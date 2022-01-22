using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanAttack : MonoBehaviour
{
    public Transform target;
    public float spd = 1;
    public TitanBehavior behaviorScript;

    private void FixedUpdate() 
    {
        if(behaviorScript.ikScript.doAttack)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, spd);
        }

    }
}
