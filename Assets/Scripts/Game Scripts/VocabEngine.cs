using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabEngine : MonoBehaviour
{

    //int answer;
    string[] questions = new string[] { "I'm cooking.", "I'm baking.", "I'm cutting.", "I'm making food.", "I'm getting ready." };
    string[] answers = new string[] { "Estoy cocinando.", "Estoy horneando.", "Estoy cortando.", "Estoy haciendo comida.", "Me estoy preparando" };
    System.Random rnd = new System.Random();
    public TMPro.TextMeshProUGUI[] buttons = new TMPro.TextMeshProUGUI[4];
    int buttonIndex;
    int QANum = 0;
    public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    //int points;
    public TMPro.TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        SetUpQuestion();
    }

    private void SetUpQuestion()
    {
        QANum = rnd.Next(5);
        question.text = questions[QANum];
        buttonIndex = rnd.Next(4);
        buttons[buttonIndex].text = answers[QANum];
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
    }

    public void Check0()
    {
        if(buttonIndex == 0)
        {
            score.text = "Correct";
            SetUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void Check1()
    {
        if (buttonIndex == 1)
        {
            score.text = "Correct";
            SetUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void Check2()
    {
        if (buttonIndex == 2)
        {
            score.text = "Correct";
            SetUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    public void Check3()
    {
        if (buttonIndex == 3)
        {
            score.text = "Correct";
            SetUpQuestion();
        }
        else
        {
            score.text = "Try Again";
        }
    }
    
}
