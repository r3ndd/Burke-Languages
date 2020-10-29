using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 10f; //value is in seconds
    int difficulty = 4;

    public TMPro.TextMeshProUGUI countdownTimer;
    private TimeSpan timePlaying;
    

    // Start is called before the first frame update
    void Start()
    {
        switch(difficulty)
        {
            case 1:
                currentTime = 0f;
                break;
            case 2:
                currentTime = 30f;
                break;
            case 3:
                currentTime = 10f;
                break;
            case 4:
                currentTime = 5f;
                break;
            default:
                currentTime = 0f;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (difficulty == 1)
        {
            currentTime += 1 * Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(currentTime);
            countdownTimer.text = timePlaying.ToString("mm':'ss");
        }
        else if (difficulty >= 2)
        {
            currentTime -= 1 * Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(currentTime);
            countdownTimer.text = timePlaying.ToString("mm':'ss");
        }
        if(currentTime <= 0)
        {
            currentTime = 0;
        }
    }
}
