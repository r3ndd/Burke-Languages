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
    private GoToScene door = new GoToScene();
    public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    public TMPro.TextMeshProUGUI score;
    public Transform cam;
    public CameraController cameraRigging;
    public ViewData view1;
    public ViewData view2;
    public ViewData view3;
    public ViewData view4;
    public ViewData view5;
    public ViewData view6;
    public ViewData view7;
    public ViewData view8;
    private QuestionHandler questionEngine;
    string answer;
    int points;
    private int playTo = 8;



    
    //public questionHandler questionEngine;

    //public Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        questionEngine = new QuestionHandler(new Question[]{
            new Question(new string[]{ "Apple", "una manzana"}, view2,
                new Question(new string[]{"cut apple", "una manzana cortada" }, view4, null)),
            new Question(new string[]{"bread", "el pan" }, view1,
                new Question(new string[]{"slice of bread", "un pedazo de pan" }, view5, null)),
            new Question(new string[]{"potato", "una papa" }, view6, null),
            new Question(new string[]{"toaster", "la tostadora"}, view3, null),
            new Question(new string[]{"plate", "el plato"}, view7,
                new Question(new string[]{"plates", "los platos" }, view8, null))}, 12);
        
        //Initialize GUI
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.GetText(questionEngine.targetLanguage);
        cameraRigging.ChangeView(questionEngine.currentQuestion.GetTarget());
        points = 0;

    }

    public void ChangeQA()
    {
        if(points == playTo)
        {
            door.ToMenu();
        }
        questionEngine.NextQuestion();
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.GetText(questionEngine.targetLanguage);
        cameraRigging.ChangeView(questionEngine.currentQuestion.GetTarget());
    }

    public void Check()
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
            ChangeQA();
            
        }
        else
        {
            score.text = "Wrong";// + "\n" + answer + "\n" + raw.Substring(0,raw.Length - 9) + "\n#\n" + raw2;
        }
    }

}
