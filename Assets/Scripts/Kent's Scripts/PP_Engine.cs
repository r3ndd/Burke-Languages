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
    public viewData view1;
    public viewData view2;
    public viewData view3;
    public viewData view4;
    public viewData view5;
    public viewData view6;
    public questionHandler questionEngine;
    string answer;
    int points;
    public int playTo = 5;



    
    //public questionHandler questionEngine;

    //public Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        questionEngine = new questionHandler(new question[]{
            new question(new string[]{ "Apple", "la manzana"}, view2,
                new question(new string[]{"cut apple", "la manzana cortada" }, view4, null)),
            new question(new string[]{"bread", "el pan" }, view1, 
                new question(new string[]{"sliced bread", "el pan cortado" }, view5, null)),
            new question(new string[]{"potato", "la papa" }, view6, null),
            new question(new string[]{"toaster", "la tostadora"}, view3, null)}, 5);
        
        //Initialize GUI
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(questionEngine.currentQuestion.getTarget());
        points = 0;

    }

    public void changeQA()
    {
        if(points == playTo)
        {
            door.toMenu();
        }
        questionEngine.nextQuestion();
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(questionEngine.currentQuestion.getTarget());
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

}
