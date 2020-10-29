using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PP_Engine : MonoBehaviour
{
    public TMPro.TextMeshProUGUI question;
    public TMPro.TextMeshProUGUI reply;
    public TMPro.TextMeshProUGUI score;
    public Button speakBtn;
    public Transform cam;
    public float startAnimDist;
    public float voiceRecogScoreThresh = 0.5f;
    public GameObject[] targets;

    private GoToScene door = new GoToScene();
    private CameraController cameraRigging;
    private Question[] questions;
    private QuestionHandler questionEngine;
    private TextToSpeech textToSpeech;
    private string answer;
    private int points;
    private int playTo = 8;

    // Start is called before the first frame update
    private void Start()
    {
        cameraRigging = cam.GetComponent<CameraController>();
        textToSpeech = GetComponent<TextToSpeech>();

        for (int i = 0; i < targets.Length; i++)
            targets[i].SetActive(false);

        string[][] questionTexts = new string[5][];
        questionTexts[0] = new string[] { "washing potato", "levando la papa" };
        questionTexts[1] = new string[] { "drying hands", "secar las manos" };
        questionTexts[2] = new string[] { "cutting potato", "cortar papa" };
        questionTexts[3] = new string[] { "cooking potato", "cocinar papas fritas" };
        questionTexts[4] = new string[] { "eating potato", "comiendo papas fritas" };

        questionEngine = new QuestionHandler(questionTexts, targets);
        answer = questionEngine.GetQuestion().GetText();
        cameraRigging.ChangeView(questionEngine.GetQuestion().GetView());
        points = 0;
    }

    private void Update()
    {
        Question question = questionEngine.GetQuestion();
        GameObject questionTarget = question.GetTarget();
        Transform questionView = question.GetView();

        if (!questionTarget.activeSelf && Vector3.Distance(cam.position, questionView.position) < startAnimDist)
            question.PlayAnimation();
    }

    public void ChangeQA()
    {
        if (points == playTo)
            door.ToMenu();

        questionEngine.NextQuestion();
        answer = questionEngine.GetQuestion().GetText();
        cameraRigging.ChangeView(questionEngine.GetQuestion().GetView());
    }

    public void Check()
    {
        string temp = reply.text.ToLower();
        byte[] bites = Encoding.Default.GetBytes(temp.Trim());
        string raw = BitConverter.ToString(bites);
        byte[] bites2 = Encoding.Default.GetBytes(answer.ToLower());
        string raw2 = BitConverter.ToString(bites2);

        if (raw.Length == 8)
        {
            score.text = "No Answer.";
            return;
        }

        if (raw2.Equals(raw.Substring(0, raw.Length - 9)))
        {
            points++;
            score.text = "Score: " + points.ToString();
            ChangeQA();
        }
        else
        {
            score.text = "Wrong";
        }
    }

    public void VoiceRecogListen()
    {
        speakBtn.enabled = false;

        textToSpeech.ListenAndScore(answer, (s) =>
        {
            speakBtn.enabled = true;

            if (s >= voiceRecogScoreThresh)
            {
                points++;
                score.text = "Score: " + points.ToString() + " " + s.ToString();
                ChangeQA();
            }
            else
                score.text = "Wrong " + s.ToString();
        });
    }

}
