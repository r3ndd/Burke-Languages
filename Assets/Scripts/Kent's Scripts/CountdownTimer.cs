using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    float duration = 0f;
    float startingTime = 10f; //value is in seconds
    static int difficulty = 1;
    bool increasing = true; //bool for font size when time is running out

    public TMPro.TextMeshProUGUI countdownTimer;
    private TimeSpan timePlaying;

    public PP_Engine pP_Engine;

    float r = 1f, g = 1f, b = 0, a = 0.6f;

    float startTime = 1f;

 //   public static AudioClip tickSound;
  //  static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        switch(difficulty)
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
        duration = currentTime;
    }

    //Difficulty getter
    public int getDifficulty()
    {
        return difficulty;
    }

    //Set time back
    public void setTime()
    {
        if (difficulty == 2)
        {
            currentTime = 31f;
        }

        else if (difficulty == 3)
        {
            currentTime = 11f;
        }

        else if (difficulty == 4)
        {
            currentTime = 6f;
        }
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
            countdownTimer.text = timePlaying.ToString("mm':'ss");

            if (currentTime > duration / 2)
            {
                g = 1f;
                r = 1f;
                b = ((currentTime - (duration / 2)) / (duration / 2));
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }

            else
            {
                g = currentTime / (duration / 2);
                r = 1f;
                b = 0f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }
            
            if (countdownTimer.color.g <= 0.5f)
            {
                if (countdownTimer.fontSize < 40f && increasing == true)
                {
                    countdownTimer.fontSize++;
                    if (countdownTimer.fontSize >= 40f)
                    {
                        increasing = false;
                    }
                }

                else if (countdownTimer.fontSize > 20f && increasing == false)
                {
                    countdownTimer.fontSize--;
                    if (countdownTimer.fontSize <= 20f)
                    {
                        increasing = true;
                    }
                }
            }

            //Reset font size
            if (countdownTimer.color.g >= 0.75f && countdownTimer.fontSize > 20)
            {
                countdownTimer.fontSize--;
            }

            
            if (currentTime <= 0)
            {
                currentTime = startTime;
            }
            /*
            else if (currentTime > 6 && currentTime < 11)
            {
                g = 1f;
                r = 1f;
                b = 0f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }
            else if (currentTime <= 6)
            {
                g = 0f;
                r = 1f;
                b = 0f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }
            else if (currentTime >= 11)
            {
                g = 1f;
                r = 1f;
                b = 1f;
                a = 1f;
                countdownTimer.color = new Color(r, g, b, a);
            }
            */
        }

    }
}
