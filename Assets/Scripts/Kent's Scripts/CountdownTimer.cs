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
    static int difficulty = 3;

    public TMPro.TextMeshProUGUI countdownTimer;
    private TimeSpan timePlaying;

    float r = 1f, g = 1f, b = 0, a = 0.6f;

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

  //          if (currentTime > 0 && currentTime <= 10)
  //          {
  //              GetComponent<AudioSource>().Play();
 //           }
        }

        if(currentTime <= 0)
        {
            currentTime = 0;
        } 
        else if (currentTime > 6 && currentTime < 11)
        {
            countdownTimer.color = new Color(r, g, b, a);
        }
        else if (currentTime <= 6 )
        {
            g = 0f;
            countdownTimer.color = new Color(r, g, b, a);
        }


    }
}
