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
        if(buttonIndex == 0)
        {
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
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check1()
    {
        if (buttonIndex == 1)
        {
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
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check2()
    {
        if (buttonIndex == 2)
        {
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
            incorrect++;
            wrong.text = incorrect.ToString();
            wrongSFX.Play();
            wrong.text = incorrect.ToString();
        }
    }
    public void check3()
    {
        if (buttonIndex == 3)
        {
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
