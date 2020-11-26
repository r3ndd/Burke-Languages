using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabEngine : MonoBehaviour
{

    //int answer;
    string[] questions = new string[] { "I am washing my hands.", "I am washing the potato.", "I am drying my hands.", "I am cutting the potato.", "I am frying the fries.", "I am serving the fries.", "I am eating the fries." };
    string[] answers = new string[] { "Me estoy lavando las manos.", "Estoy lavando la papa.", "Me estoy secando las manos.", "Estoy cortando la papa.", "Estoy cocinando las papas fritas.", "Estoy sirviendo las papas.", "Estoy comiendo las papas." };
    System.Random rnd = new System.Random();
    public TMPro.TextMeshProUGUI[] buttons = new TMPro.TextMeshProUGUI[4];
    public viewData[] views = new viewData[7];
    public AudioSource[] voiceClips = new AudioSource[8];
    public cameraController cameraRigging;
    int buttonIndex;
    int QANum = 0;
    int possibleNext = -1;
    //public TMPro.TextMeshProUGUI question;
    //public TMPro.TextMeshProUGUI reply;
    private int correct = 0;
    private int incorrect = 0;
    public TMPro.TextMeshProUGUI right;
    public TMPro.TextMeshProUGUI wrong;
    public GameObject water;
    public AudioSource rightSFX;
    public AudioSource wrongSFX;
    bool clipQueued = false;
    bool checking = false;
    bool correctAnswer = false;
    bool fluctuate = true;
    int fluctuatenum;


    // Start is called before the first frame update
    void Start()
    {
        foreach(viewData view in views)
        {
            view.toggleAnime(false);
        }
        voiceClips[7].Play();
        setUpQuestion();
        water.SetActive(true);
        fluctuatenum = 10;
    }

    void Update()
    {
        Debug.Log(correctAnswer + " " + checking + fluctuatenum);

        if (correctAnswer == true && checking == true && fluctuatenum > 0)
        {
            //Make score glow green
            if (right.color.r > 0.3f && fluctuate == true)
            {
                right.color = new Color(right.color.r - 0.1f, 1, right.color.b - 0.1f, 1);

                if (right.color.r <= 0.3f)
                {
                    fluctuate = false;
                    fluctuatenum--;
                }
            }

            //Set back to white
            else if (right.color.r < 1 && fluctuate == false)
            {
                right.color = new Color(right.color.r + 0.1f, 1, right.color.b + 0.1f, 1);

                if (right.color.r >= 1)
                {
                    fluctuate = true;
                    fluctuatenum--;
                }
            }
        }

        else if (correctAnswer == false && checking == true && fluctuatenum > 0)
        {
            //Make score glow red
            if (wrong.color.g > 0.3f && fluctuate == true)
            {
                wrong.color = new Color(1, wrong.color.g - 0.1f, wrong.color.b - 0.1f, 1);

                if (wrong.color.g <= 0.3f)
                {
                    fluctuate = false;
                    fluctuatenum--;
                }
            }

            //Set back to white
            else if (wrong.color.g < 1 && fluctuate == false)
            {
                wrong.color = new Color(1, wrong.color.g + 0.1f, wrong.color.b + 0.1f, 1);

                if (wrong.color.g >= 1)
                {
                    fluctuate = true;
                    fluctuatenum--;
                }
            }
        }

        if (fluctuatenum <= 0)
        {
            fluctuatenum = 10;
            checking = false;
        }
    }

    private void setUpQuestion()
    {

        water.SetActive(true);
        views[QANum].toggleAnime(false);
        do
        {
            possibleNext = rnd.Next(7);
        } while (possibleNext == QANum);
        QANum = possibleNext;
        buttonIndex = rnd.Next(4);
        buttons[buttonIndex].text = answers[QANum];

        StartCoroutine(activateAnime());
        cameraRigging.changeView(views[QANum]);
        int tick = 0;
        int temp1 = -1;
        int temp2 = -1;
        int temp3 = -1;
        if(tick == buttonIndex) tick++;
        while (temp1 == -1)
        {
            temp1 = rnd.Next(5);
            if (temp1 == QANum) temp1 = -1;
            else buttons[tick].text = answers[temp1];
        }
        tick++;
        if (tick == buttonIndex) tick++;
        while (temp2 == -1)
        {
            temp2 = rnd.Next(5);
            if (temp2 == QANum || temp2 == temp1) temp2 = -1;
            else buttons[tick].text = answers[temp2];
        }
        tick++;
        if (tick == buttonIndex) tick++;
        while (temp3 == -1)
        {
            temp3 = rnd.Next(5);
            if (temp3 == QANum || temp3 == temp1 || temp3 == temp2) temp3 = -1;
            else buttons[tick].text = answers[temp3];
        }

    }

    public void check0()
    {
        checking = true;
        if(buttonIndex == 0)
        {
            correctAnswer = true;
            correct++;
            right.text = correct.ToString();
            rightSFX.Play();
            voiceClips[QANum].Play();
            clipQueued = true;
            StartCoroutine(queryUser());
            setUpQuestion();
        }
        else
        {
            correctAnswer = false;
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check1()
    {
        checking = true;
        if (buttonIndex == 1)
        {
            correctAnswer = true;
            correct++;
            right.text = correct.ToString();
            rightSFX.Play();
            voiceClips[QANum].Play();
            clipQueued = true;
            StartCoroutine(queryUser());
            setUpQuestion();
        }
        else
        {
            correctAnswer = false;
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check2()
    {
        checking = true;
        if (buttonIndex == 2)
        {
            correctAnswer = true;
            correct++;
            right.text = correct.ToString();
            rightSFX.Play();
            voiceClips[QANum].Play();
            clipQueued = true;
            StartCoroutine(queryUser());
            setUpQuestion();
        }
        else
        {
            correctAnswer = false;
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check3()
    {
        checking = true;
        if (buttonIndex == 3)
        {
            correctAnswer = true;
            correct++;
            right.text = correct.ToString();
            rightSFX.Play();
            voiceClips[QANum].Play();
            clipQueued = true;
            StartCoroutine(queryUser());
            setUpQuestion();
        }
        else
        {
            correctAnswer = false;
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }

    IEnumerator queryUser()
    {
        yield return new WaitForSeconds(2);
        voiceClips[7].Play();
        clipQueued = false;
    }

    IEnumerator activateAnime()
    {
        yield return new WaitForSeconds(.7f);
        views[QANum].toggleAnime(true);
    }
    
}
