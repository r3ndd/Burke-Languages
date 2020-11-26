using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public CountdownTimer timer;
    private int responseType;
    public AudioSource rightSFX;
    public AudioSource wrongSFX;
    public viewData[] views = new viewData[7];
    public AudioSource[] voiceClips = new AudioSource[6];
    private questionHandler questionEngine;
    string answer;
    int points;
    private int playTo = 7;
    private SpeechToText speechToText;
    public GameObject water;
    public GameObject voiceButton;
    public GameObject typeButton;
    public GameObject typeField;
    public GameObject x1;
    public GameObject x2;
    public GameObject x3;
    private int lives = 3;
    private bool checking = false;
    private bool correct = false;
    private bool fluctuate = true;
    private int fluctuatenum;



    //public questionHandler questionEngine;

    //public Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        fluctuatenum = 10;
        responseType = PlayerPrefs.GetInt("responseType");
        if (responseType == 0)
        {
            //voiceButton.SetActive(false);
        }
        else if(responseType == 1)
        {
            typeButton.SetActive(false);
            typeField.SetActive(false);
        }
        voiceClips[7].Play();
        speechToText = GetComponent<SpeechToText>();



        water.SetActive(true);
        foreach (viewData view in views)
        {
            view.toggleAnime(false);
        }

        questionEngine = new questionHandler(new question[]{
            new question(new string[]{ "I am washing my hands.", "Me estoy lavando las manos."}, 0,
                new question(new string[]{ "I am washing the potato.", "Estoy lavando la papa."}, 1,
                    new question(new string[]{"I am drying my hands.", "Me estoy secando las manos." }, 2,
                        new question(new string[]{"I am cutting the potato.", "Estoy cortando la papa." }, 3,
                            new question(new string[]{"I am cooking the potato.", "Estoy cocinando la papa."}, 4,
                                new question(new string[]{ "I am serving the fries.", "Estoy sirviendo las papas."}, 5,
                                    new question(new string[]{"I am eating the fries.", "Estoy comiendo las papas."}, 6, null)))))))}, 7);

        //Initialize GUI
        //question.text = questionEngine.currentQuestion.getText(0);
        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(views[getIndex()]);
        views[getIndex()].toggleAnime(true);
        points = 0;
        water.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            check();
        }

        if (correct == true && checking == true && fluctuatenum > 0)
        {
            //Make score glow green
            if (score.color.r > 0.3f && fluctuate == true)
            {
                score.color = new Color(score.color.r - 0.1f, 1, score.color.b - 0.1f, 1);
                    
                if (score.color.r <= 0.3f)
                {
                    fluctuate = false;
                    fluctuatenum--;
                }
            }

            //Set back to white
            else if (score.color.r < 1 && fluctuate == false)
            {
                score.color = new Color(score.color.r + 0.1f, 1, score.color.b + 0.1f, 1);

                if (score.color.r >= 1)
                {
                    fluctuate = true;
                    fluctuatenum--;
                }
            }
        }

        else if (correct == false && checking == true && fluctuatenum > 0)
        {
                //Make score glow red
                if (score.color.g > 0.3f && fluctuate == true)
                {
                    score.color = new Color(1, score.color.g - 0.1f, score.color.b - 0.1f, 1);

                    if (score.color.g <= 0.3f)
                    {
                        fluctuate = false;
                        fluctuatenum--;
                    }
                }
                
                //Set back to white
                else if (score.color.g < 1 && fluctuate == false)
                {
                    score.color = new Color(1, score.color.g + 0.1f, score.color.b + 0.1f, 1);

                    if (score.color.g >= 1)
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

    public void changeQA()
    {
        if (points == playTo)
        {
            door.toMenu();
        }
        views[getIndex()].toggleAnime(false);
        questionEngine.nextQuestion();

        if (questionEngine.currentQuestion == null)
        {
            endGame();
            return;
        }

        answer = questionEngine.currentQuestion.getText(questionEngine.targetLanguage);
        cameraRigging.changeView(views[getIndex()]);
        voiceClips[7].Play();
        views[getIndex()].toggleAnime(true);
    }

    public void check()
    {
        checking = true;
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
            rightSFX.Play();
            correct = true;
            StartCoroutine(timer.resetTimer());
            timer.changeQueued = true;
            points++;
            score.text = "Score: " + points.ToString();          
            voiceClips[getIndex()].Play(1);
            StartCoroutine(sitTight());
        }
        else
        {
            wrongSFX.Play();
            correct = false;
            score.text = "Wrong";// + "\n" + answer + "\n" + raw.Substring(0,raw.Length - 9) + "\n#\n" + raw2;

            //Make score glow red
            //score.text
        }
    }

    public void checkByVoice()
    {
        speechToText.ListenAndScore(answer, (_score) =>
        {
            score.text = _score.ToString();
            if (_score > 0.5f)
            {
                score.text = "Correct: " + _score.ToString();
                rightSFX.Play();
                changeQA();
            }
            else
            {
                wrongSFX.Play();
                score.text = "Incorrect: " + _score.ToString();
            }
        });
    }

    private int getIndex()
    {
        water.SetActive(true);
        return questionEngine.currentQuestion.getTarget();
    }

    public IEnumerator timeIs0()
    {
        lives--;

        if (lives < 3)
        {
            x1.GetComponent<TextMeshProUGUI>().color = new Color32(120, 0, 0, 255);

            if (lives < 2)
            {
                x2.GetComponent<TextMeshProUGUI>().color = new Color32(120, 0, 0, 255);

                if (lives < 1)
                    x3.GetComponent<TextMeshProUGUI>().color = new Color32(120, 0, 0, 255);
            }
        }

        wrongSFX.Play();
        yield return new WaitForSeconds(1);
        voiceClips[getIndex()].Play();
        yield return new WaitForSeconds(3);

        if (lives > 0)
            changeQA();
        else
            endGame();
    }

    IEnumerator sitTight()
    {
        yield return new WaitForSeconds(3);
        changeQA();
    }

    public void endGame()
    {
        Debug.Log("GAME IS OVER!");
    }
}
