using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RingControl : MonoBehaviour
{
    //rings
    public GameObject[] rings;
    public float ringNumb = 0;

    //timer
    public float elapsedTime;
    public bool timeGoing;
    TimeSpan timePlaying;

    public Text timerUI;

    void Start()
    {
        foreach(GameObject ring in rings)
        {
            ring.SetActive(false);
        }

        rings[0].SetActive(true);
    }

    void Update()
    {
        if(timeGoing)
        {
            //update time
            elapsedTime += Time.deltaTime;

            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");

            timerUI.text = timePlayingStr;
        }
    }
    
    public void StartFirstRing()
    {
        StartTimer();
        GoToNextRing();
    }
    
    public void GoToNextRing()
    {
        rings[(int)ringNumb].SetActive(false);
        ringNumb++;
        rings[(int)ringNumb].SetActive(true);

    }

    public void FinishLastRing()
    {
        FinishTimer();
    }

    void StartTimer()
    {
        Debug.Log("START TIMER");
        timeGoing = true;
    }

    void FinishTimer()
    {
        Debug.Log("FINISH TIMER");
        timeGoing = false;
    }
}
