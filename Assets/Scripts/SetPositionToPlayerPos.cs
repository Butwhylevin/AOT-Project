using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetPositionToPlayerPos : MonoBehaviour
{
    public Transform playerObj;

    private void FixedUpdate() 
    {
        transform.position = playerObj.transform.position;
        transform.rotation = playerObj.transform.rotation;
    }

    private void Update() 
    {
        transform.position = playerObj.transform.position;
        transform.rotation = playerObj.transform.rotation;
    }
}
