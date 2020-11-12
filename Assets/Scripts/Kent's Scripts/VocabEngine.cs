using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabEngine : MonoBehaviour
{

    //int answer;
    string[] questions = new string[] { "I am washing the potato.", "I am drying my hands.", "I am cutting the potato.", "I am frying the fries.", "I am eating the fries." };
    string[] answers = new string[] { "Estoy lavando la papa.", "Estoy secando mis manos.", "Estoy cortando la papa.", "Estoy friendo las papas fritas.", "Estoy comiendo las papas fritas." };
    System.Random rnd = new System.Random();
    public TMPro.TextMeshProUGUI[] buttons = new TMPro.TextMeshProUGUI[4];
    public viewData[] views = new viewData[5];
    public cameraController cameraRigging;
    int buttonIndex;
    int QANum = 0;
    int possibleNext = -1;
    //public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    //int points;
    public TMPro.TextMeshProUGUI score;
    public GameObject water;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (viewData view in views)
        {
            view.toggleAnime(false);
            // debug.log("just now doing this.");
        }
        setUpQuestion();
        water.SetActive(true);
        //for (int i = 0; i < 100; i++)
        //{
        //    check0();
        //    check1();
        //    check2();
        //    check3();
        //}
    }

    private void setUpQuestion()
    {

        water.SetActive(true);
        views[QANum].toggleAnime(false);
        //Debug.Log(views[QANum].anime.activeSelf);
        do
        {
            possibleNext = rnd.Next(5);
        } while (possibleNext == QANum);
        QANum = possibleNext;
        //question.text = "";
        buttonIndex = rnd.Next(4);
        buttons[buttonIndex].text = answers[QANum];
        views[QANum].toggleAnime(true);
        //foreach (viewData view in views)
        //{
        //    Debug.Log(view.anime.activeSelf);
        //}
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
        //question.text = "Done.";
        //foreach (viewData view in views)
        //{
        //    Debug.Log(view.anime.activeSelf);
        //}
    }

    public void check0()
    {
        if(buttonIndex == 0)
        {
            score.text = "Correct";
            setUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void check1()
    {
        if (buttonIndex == 1)
        {
            score.text = "Correct";
            setUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void check2()
    {
        if (buttonIndex == 2)
        {
            score.text = "Correct";
            setUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void check3()
    {
        if (buttonIndex == 3)
        {
            score.text = "Correct";
            setUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    
}
