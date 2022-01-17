using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //various keys
        if(Input.GetKeyDown(KeyCode.R))
        {
            //restart room
            Application.LoadLevel (Application.loadedLevel);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //return to menu
            Application.LoadLevel ("MainMenu");
        }

    }
}
