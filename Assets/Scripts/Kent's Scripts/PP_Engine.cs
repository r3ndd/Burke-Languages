using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

//this approach doesn't seem like it will work
//instead try making a component to attach to a game object that will provide the needed data and form a node in a linked list in a series
//create an end for the series

public class question //This is intended to encapsulate a single question/translation
{
    string[] questionText;//This array contains the translations in different languages and should always be in the following order
    //ENGLISH, SPANISH, ... TBD
    public Transform targetCameraLocation; //The camera will move to this location when the question is active
    public Transform cameraTarget; //The camera will face this location when the question is active
    public question(string[] texts)
    {
        questionText = texts;
    }

    public string getText(int languageIndex)
    {
        return questionText[languageIndex];
    }

    public Transform getTarget()
    {
        return targetCameraLocation;
    }
}

public class questionSeries //This class is for handling a series of related questions in a specific order such as "I am slicing bread." before "I am toasting bread."
{
    public question[] series;
    int current = -1;

    public questionSeries(question first)
    {
        series[0] = first;
    }

    public void addQuestion(question adding)
    {
        series[series.Length] = adding;
    }
    //public void nextQuestion()
    //{
    //    current++;
    //}

    public question getQuestion()
    {
        if(current + 1 == series.Length)
        {
            return null;
        }
        return series[current++];
    }
}

public class questionHandler //This class is for interfacing with a game controller to get the proper question data
{
    private int nativeLanguage;
    private int targetLanguage;
    private question currentQuestion;
    public questionSeries[] allQuestions;
    System.Random rng = new System.Random();
    private int[] usedNumbers; //series that have already been used

    public questionHandler(int nat, int targ, questionSeries[] al)
    {
        nativeLanguage = nat;
        targetLanguage = targ;
        allQuestions = al;
        usedNumbers[usedNumbers.Length - 1] = rng.Next() % allQuestions.Length;
        currentQuestion = allQuestions[usedNumbers[usedNumbers.Length - 1]].getQuestion();
    }

    public void next()
    {
        currentQuestion = allQuestions[usedNumbers[usedNumbers.Length - 1]].getQuestion();
        if (currentQuestion == null)
        {
            int num = rng.Next() % allQuestions.Length;
            bool used = true;
            while (used)
            {
                used = false;
                foreach (int i in usedNumbers)
                {
                    if(i == num)
                    {
                        num = rng.Next() % allQuestions.Length;
                        used = true;
                        break;
                    }
                }
            }
            usedNumbers[usedNumbers.Length] = num;
            currentQuestion = allQuestions[usedNumbers.Length - 1].getQuestion();
        }
    }

    public string getTarget()
    {
        return currentQuestion.getText(targetLanguage);
    }

    public string getNative()
    {
        return currentQuestion.getText(nativeLanguage);
    }

    public Transform getCameraTargetLocation()
    {
        return currentQuestion.targetCameraLocation;
    }

    public Transform getCameraTarget()
    {
        return currentQuestion.cameraTarget;
    }
}

public class PP_Engine : MonoBehaviour
{

    string answer = "Estoy cocinando.";
    string[] questions = new string[] { "I'm cooking.", "I'm baking.", "I'm cutting.", "I'm making food.", "I'm getting ready." };
    string[] answers = new string[] { "Estoy cocinando.", "Estoy horneando.", "Estoy cortando.", "Estoy haciendo comida.", "Me estoy preparando"};
    System.Random rnd = new System.Random(5);
    int QANum = 0;
    public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    int points;
    public TMPro.TextMeshProUGUI score;
    public Transform cam;

    public questionHandler questionEngine;

    //public Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        questionSeries ingredients = new questionSeries(new question(new string[] { "apple", "manzana"}));
        ingredients.addQuestion(new question(new string[] { "potato", "papa" }));
        ingredients.addQuestion(new question(new string[] { "bread", "pan" }));
        questionEngine = new questionHandler(0, PlayerPrefs.GetInt("tLanguage", 1), new questionSeries[] { ingredients });
        cam.position = questionEngine.getCameraTargetLocation().position;
        cam.LookAt(questionEngine.getCameraTarget());
        QANum = rnd.Next(5);
        points = 0;
    }

    public void changeQA()
    {
        QANum = rnd.Next(5);
        question.text = questions[QANum];
        answer = answers[QANum];
        Debug.Log(cam.rotation.y < 90);
        if (cam.position.x < 50)
        {
            Vector3 temp = new Vector3(8.5f, 0f, 0f);
            cam.position += temp;
        }
        else if (true)
        {
            Debug.Log("Turning?");
        }    
        
    }

    public void check()
    {
        string temp = reply.text;
        byte[] bites = Encoding.Default.GetBytes(temp.Trim());
        string raw = BitConverter.ToString(bites);
        byte[] bites2 = Encoding.Default.GetBytes(answer);
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
            score.text = "Wrong" + "\n" + answer + "\n" + raw.Substring(0,raw.Length - 9) + "\n#\n" + raw2;
        }
    }

}
