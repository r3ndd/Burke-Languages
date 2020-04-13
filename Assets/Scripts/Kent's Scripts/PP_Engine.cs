using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;


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

    // Start is called before the first frame update
    void Start()
    {
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
        else if(true)
        {
            Debug.Log("Turning?");
            cam.rotation.
        
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
