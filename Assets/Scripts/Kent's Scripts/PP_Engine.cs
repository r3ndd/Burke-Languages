using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

//this approach doesn't seem like it will work
//instead try making a component to attach to a game object that will provide the needed data and form a node in a linked list in a series
//create an end for the series



public class PP_Engine : MonoBehaviour
{
    private goToScene door = new goToScene();
    public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    public TMPro.TextMeshProUGUI score;
    public Transform cam;
    public cameraController cameraRigging;
    //public viewData view1;
    //public viewData view2;
    //public viewData view3;
    //public viewData view4;
    //public viewData view5;
    //public viewData view6;
    //public viewData view7;
    //public viewData view8;
    public viewData[] views = new viewData[5];
    private questionHandler questionEngine;
    string answer;
    int points;
    private int playTo = 5;
    private SpeechToText speechToText;
    public GameObject water;



    
    //public questionHandler questionEngine;

    //public Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        speechToText = GetComponent<SpeechToText>();

        water.SetActive(true);
        foreach (viewData view in views)
        {
            view.toggleAnime(false);
        }

        questionEngine = new questionHandler(new question[]{
            new question(new string[]{ "I am washing the potato.", "Estoy lavando la papa."}, 0, 
                new question(new string[]{"I am drying my hands.", "Estoy secando mis manos." }, 1, 
                    new question(new string[]{"I am cutting the potato.", "Estoy cortando la papa." }, 2, 
                        new question(new string[]{"I am frying the fries.", "Estoy friendo las papas fritas."}, 3, 
                            new question(new string[]{"I am eating the fries.", "Estoy comiendo las papas fritas."}, 4, null)))))}, 5);
        
        //Initialize GUI
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(views[getIndex()]);
        views[getIndex()].toggleAnime(true);
        points = 0;
        water.SetActive(true);
    }

    public void changeQA()
    {
        if(points == playTo)
        {
            door.toMenu();
        }
        views[getIndex()].toggleAnime(false);
        questionEngine.nextQuestion();
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(views[getIndex()]);
        views[getIndex()].toggleAnime(true);
    }

    public void check()
    {
        string temp = reply.text.ToLower();
        byte[] bites = Encoding.Default.GetBytes(temp.Trim());
        string raw = BitConverter.ToString(bites);
        byte[] bites2 = Encoding.Default.GetBytes(answer.ToLower());
        string raw2 = BitConverter.ToString(bites2);
        if(raw.Length == 8)
        {
            score.text = "No Answer.";
            return;
        }
        if (raw2.Equals(raw.Substring(0,raw.Length - 9)))
        {
            points++;
            score.text = "Score: " + points.ToString();
            changeQA();
            
        }
        else
        {
            score.text = "Wrong";// + "\n" + answer + "\n" + raw.Substring(0,raw.Length - 9) + "\n#\n" + raw2;
        }
    }

    public void checkByVoice()
    {
        score.text = answer;

        speechToText.GetSpeech((text) =>
        {
            score.text = text;
        });

        //speechToText.ListenAndScore(answer, (_score) =>
        //{
        //    score.text = _score.ToString();
        //    if (_score > 0.5f)
        //    {
        //        score.text = "Correct: " + _score.ToString();
        //        changeQA();
        //    }
        //    else
        //    {
        //        score.text = "Incorrect: " + _score.ToString();
        //    }
        //});
    }

    private int getIndex()
    {
        water.SetActive(true);
        return questionEngine.currentQuestion.getTarget();
    }

    public void timeIs0()
    {
        return;
    }

}
