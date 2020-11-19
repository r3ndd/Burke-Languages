using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    float startingTime = 10f; //value is in seconds
    static int difficulty = 3;
    public PP_Engine time0Handler;

    public TMPro.TextMeshProUGUI countdownTimer;
    private TimeSpan timePlaying;

    float r = 1f, g = 1f, b = 0, a = 0.6f;

    float startTime = 1f;
    public AudioSource tickSound;
    bool tick = true;
    public bool changeQueued = false;
    string initialTime = "00:10";
    string nextTime = "00:10";

    // Start is called before the first frame update
    void Start()
    {
        switch (difficulty)
        {
            case 1:
                currentTime = 0f;
                break;
            case 2:
                currentTime = 31f;
                break;
            case 3:
                currentTime = 11f;
                break;
            case 4:
                currentTime = 6f;
                break;
            default:
                currentTime = 0f;
                break;
        }
        //      text = countdownTimer.getComponent<Text>();
        //      text.color = new Color(r, g, b, a);

        startTime = currentTime;
    }

    public void ChangeDiffOne()
    {
        difficulty = 1;
    }

    public void ChangeDiffTwo()
    {
        difficulty = 2;
    }

    public void ChangeDiffThree()
    {
        difficulty = 3;
    }

    public void ChangeDiffFour()
    {
        difficulty = 4;
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
            if (!changeQueued)
            {
                countdownTimer.text = timePlaying.ToString("mm':'ss");
            }
            else
            {
                currentTime = 0;
                countdownTimer.text = timePlaying.ToString("mm':'ss");
            }
            if (currentTime <= 0 && !changeQueued)
            {
                StartCoroutine(time0Handler.timeIs0());
                StartCoroutine(resetTimer());
                changeQueued = true;
            }
            else if (currentTime > 6 && currentTime < 11)
            {
                g = 1f;
                r = 1f;
                b = 0f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);

                nextTime = countdownTimer.text;
                if (nextTime != initialTime)
                {
                    tick = true;
                    initialTime = nextTime;
                }

                while (tick)
                {
                    tickSound.Play();
                    tick = false;
                }

            }
            else if (currentTime <= 6)
            {
                g = 0f;
                r = 1f;
                b = 0f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);

                nextTime = countdownTimer.text;
                if (nextTime != initialTime)
                {
                    tick = true;
                    initialTime = nextTime;
                }

                while (tick)
                {
                    tickSound.Play();
                    tick = false;
                }

            }
            else if (currentTime >= 11)
            {
                g = 1f;
                r = 1f;
                b = 1f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }

        }

    }

    public IEnumerator resetTimer()
    {
        yield return new WaitForSeconds(6);
        currentTime = startTime;
        changeQueued = false;
    }

}