using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToSandbox()
    {
        SceneManager.LoadScene("sandbox");
    }
    public void GoToCourse()
    {
        SceneManager.LoadScene("course");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
