using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DestroyTower : MonoBehaviour
{
    public GameObject brokenTower;

    [Header("Camera Shake")]
    public float magn;
    public float rough, fadeIn, fadeOut;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            Break();
        }
    }

    private void Break()
    {
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
        Instantiate(brokenTower, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
