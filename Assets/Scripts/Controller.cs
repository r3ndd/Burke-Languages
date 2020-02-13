﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class Controller : MonoBehaviour
{
    public Transform screen;

    public GameObject action;

    public int numberOfActions;
    public Difficulty difficulty;
    public Game1Player[] players;


    public string[] wordbank;


    public string wordRecognized;
    public bool recognizedNewWord = false;

    //////////////////////////////////////////
    public bool gameStart = false;
    public bool redButtonIsPushed;
    public Button redButton;
    public Sprite redButton1;
    public Sprite redButton2;
    public Image micIcon;
    public float value1 = (float)0.22301, value2 = (float)0.21500, value3 = (float)1.0;
    public KeywordRecognizer keywordRecognizer;
    public Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public float startTime;
    public bool voiceActive;

    public InputField InputFieldAnswer;
    public Button confirmTextButton;

    ///////////////////////////////////////

    public float timer;
    public TextMeshPro timerText;


    public ActionObject[] actionObjects;


    public Dictionary<string, List<GameObject>> wordToObject;

    public Dictionary<string, WordSelection> wordSelected;


    public struct WordSelection
    {
        public bool selected;
        public Game1Player player;

        public void Deselect()
        {
            selected = false;
        }

        public WordSelection(bool selected, Game1Player player)
        {
            this.selected = selected;
            this.player = player;
        }

    }

    float duration = 2f;

    float speed;

    bool gameRunning = false;

    public TextMeshPro startText;


    public static Controller instance;


    public Transform arenaContainer;

    public AudioSource correctAnswerSource;


    public Image fadeScreen;


    public List<CharacterObject> listOfCharacters;

    public AudioSource endGame;

    public ParticleSystem correctParticles;


    public SpeechBubble[] bubbles;

    [System.Serializable]
    public struct SpeechBubble
    {
        public GameObject speechBubble;
        public TextMeshPro bubbleText;
    }



    public TextMeshPro pointAddPrefab;


    private void Awake()
    {
        instance = this;
    }



    // Use this for initialization
    void Start ()
    {

        voiceActive = false;

        //------------------- Start of Voice Recognition Section --------------------

        //Adding to dictionary
        //Do Commands
        actions.Add("brush", Brush);
        actions.Add("dry", Dry);
        actions.Add("tie", Tie);
        actions.Add("wash", Wash);
        actions.Add("wear", Wear);
        //Don't commands
        actions.Add("don't brush", DontBrush);
        actions.Add("don't dry", DontDry);
        actions.Add("don't tie", DontTie);
        actions.Add("don't wash", DontWash);
        actions.Add("don't wear", DontWear);
        //Do not commands
        actions.Add("do not brush", DoNotBrush);
        actions.Add("do not dry", DoNotDry);
        actions.Add("do not tie", DoNotTie);
        actions.Add("do not wash", DoNotWash);
        actions.Add("do not wear", DoNotWear);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;

        //------------------- End of Voice Recognition Section ----------------------

        if (gameStart == false)
        {
            gameStart = true;
            InputFieldAnswer.transform.localScale = new Vector3(0, 0, 0);
            confirmTextButton.transform.localScale = new Vector3(0, 0, 0);
            micIcon.transform.localScale = new Vector3(0, 0, 0);
        }
        redButton.GetComponent<Button>();
        redButtonIsPushed = false;
        wordToObject = new Dictionary<string, List<GameObject>>();
        wordSelected = new Dictionary<string, WordSelection>();


        switch (difficulty)
        {
            case Difficulty.Easy:
                speed = 0.2f;
                break;
            case Difficulty.Intermediate:
                speed = 1f;
                break;
            case Difficulty.Difficult:
                speed = 1.25f;
                break;
            default:
                speed = 1f;
                break;
        }


        


        if (MenuSelection.instance)
        {
            MenuSelection.instance.FadeIn(1f);
        }


        if(IntroController.instance)
        {
            listOfCharacters = IntroController.instance.charactersInGame;
        }

        if(AudioManager.instance)
        {
            AudioManager.instance.musicSource.clip = AudioManager.instance.game1;
            AudioManager.instance.musicSource.Play();
        }


        wordbank = new string[5];
        for (int i = 0; i < 5; i++)
        {
            wordbank[i] = actionObjects[i].presentSimpleSentence;
        }



        for (int i = 0; i < 4; i++)
        {
            print("here");
            players[i].character = listOfCharacters[i];
            players[i].ChangeSprite();
        }

        foreach (var item in actionObjects)
        {
            wordSelected.Add(item.presentSimpleSentence, new WordSelection());
            wordToObject.Add(item.presentSimpleSentence, new List<GameObject>());
        }

        for (int i = 0; i < bubbles.Length; i++)
        {
            bubbles[i].speechBubble.SetActive(false);
            bubbles[i].speechBubble.GetComponent<SpriteRenderer>().color = players[i].character.color;
        }


        InitGame();


        //var outline = Instantiate(new GameObject(), players[0].transform.position, Quaternion.identity, players[0].transform);
        //outline.transform.localScale += Vector3.one * 0.1f;
        //outline.transform.position += Vector3.back * 0.1f + Vector3.down * 0.12f;
        //var outlineRend = outline.AddComponent<SpriteRenderer>();
        //outlineRend.sprite = players[0].character.silhouette;
        //outlineRend.color = Color.yellow;








    }
	

    void PlaySoundCorrect()
    {
        correctAnswerSource.Play();
    }



    public void SetTimer(float time)
    {
        timer = time;
    }

	// Update is called once per frame
	void Update ()
    {
        float t = Time.time - startTime;
        if (voiceActive == true)
        {
            //Checking if 5 seconds has passed since clicking the red button
            if (t >= 5f)
            {
                showAnswerTextbox();
                hideMicIcon();
                keywordRecognizer.Stop();
                voiceActive = false;
            }

        }
        

            if (Input.GetKeyDown(KeyCode.P))
        {
            timer = 2f;
        }

        

        if (Input.GetKeyDown(KeyCode.Z))
        {
            MakeGuess(0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            MakeGuess(1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            MakeGuess(2);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MakeGuess(3);
        }
        

        if (gameRunning)
        {
            timer -= Time.deltaTime;
        }

        if(timerText.gameObject.activeSelf || timerText != null)
            timerText.text = Mathf.RoundToInt(timer).ToString();

        if (timer <= 0f)
        {
            EndGame();
        }

        if (gameRunning)
        {
            //check if voice input has been activated i.e. red button is clicked. and stop npc guess if button clicked
            if (redButtonIsPushed == false)
            {
                redButton.transform.GetComponent<RectTransform>().rect.Set(value1, value1, value1, value1);
                //npc guess answer
                if (Time.frameCount % 60 == 0)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        if (!players[i].waving)
                        {
                            if (UnityEngine.Random.value <= 0.1)
                            {
                                MakeGuess(i);
                            }
                        }
                    }
                }
            }
        }

        foreach (var item in players)
        {
            if(item.waving)
            {
                if(item.waveTimer <= 0f)
                {
                    item.waving = false;
                    item.characterRender.sprite = item.character.behind;

                    bubbles[item.i].speechBubble.SetActive(false);

                    wordSelected[item.guessWord] = new WordSelection(false, wordSelected[item.guessWord].player);
                    /////////////////////////////////////////////////////////////////////////////
                    print(wordSelected[item.guessWord].selected);

                    if (wordToObject.ContainsKey(item.guessWord))
                    {
                        var tempList = new List<GameObject>();

                        var pointsAdded = Instantiate(pointAddPrefab, item.scoreText.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                        var pointAmt = 0;
                        pointsAdded.text = "";
                        pointsAdded.color = item.character.color;

                        foreach (var action in wordToObject[item.guessWord])
                        {
                            item.score += 10;
                            pointAmt += 10;
                            var particles = Instantiate(correctParticles, action.transform.position, Quaternion.Euler(-90,0,0),action.transform.parent);
                            particles.startColor = item.character.color;
                            particles.Play();
                            Destroy(particles.gameObject, 2f);
                            tempList.Add(action);
                        }

                        if(pointAmt != 0)
                            pointsAdded.text = "+" + pointAmt.ToString();

                        if(wordToObject[item.guessWord].Count > 0)
                            PlaySoundCorrect();

                        foreach (var it in tempList)
                        {
                            wordToObject[item.guessWord].Remove(it);
                            Destroy(it.gameObject);

                            SpawnAction();
                        }
                        
                    }

                    item.scoreText.text = item.score.ToString();
                }
            }
        }

    }






    int RandomPosition(string key)
    {
        int amount = wordToObject[key].Count;
        print(amount);

        foreach (GameObject item in wordToObject[key].ToArray())
        {
            float x = UnityEngine.Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
            float y = UnityEngine.Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

            item.transform.position = new Vector3(x,y);

            var temp = item.GetComponent<movement>().actionObj.presentSimpleSentence;


            wordToObject[temp].Remove(item);

            item.GetComponent<movement>().SetUp(actionObjects[UnityEngine.Random.Range(0,actionObjects.Length)]);

            temp = item.GetComponent<movement>().actionObj.presentSimpleSentence;

            wordToObject[temp].Add(item);
        }

        return amount;
    }


    void EndGame()
    {
        if (gameRunning)
        {
            gameRunning = false;
            MenuSelection.instance.GoToMenuScene(3f, MenuSelection.instance.selectedMinigame.sceneName);
            print(MenuSelection.instance.selectedMinigame.sceneName);
            timerText.gameObject.SetActive(false);
            startText.gameObject.SetActive(true);
            startText.text = "Finish!";
            MenuSelection.instance.selectedMinigame.Scores.Add(players[0].score);
            endGame.Play();
        }
        
        //SceneManager.LoadScene(menuScene.name);
    }


    void InitGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        #region wait 1 second before starting the game
        // timer for moving the menu
        float journey = 0f;

        // keep adjusting the position while there is time
        while (journey <= duration)
        {
            // add to timer
            journey = journey + Time.deltaTime;
            yield return null;
        }
        #endregion

        for (int i = 0; i < numberOfActions; i++)
        {
            SpawnAction();

        }

        gameRunning = true;

        startText.gameObject.SetActive(false);
    }


    void SpawnAction()
    {
        float x = UnityEngine.Random.Range(screen.position.x - 2f, screen.position.x + 2f);
        float y = UnityEngine.Random.Range(screen.position.y - 3f, screen.position.y + 0.5f);

        var obj = Instantiate(action, new Vector3(screen.transform.position.x + x, screen.transform.position.y + y, screen.transform.position.z - 0.01f), Quaternion.identity, arenaContainer).GetComponent<movement>();

        obj.speed = speed;
        obj.actionObj = actionObjects[UnityEngine.Random.Range(0, actionObjects.Length)];

        var sentence = obj.actionObj.presentSimpleSentence;

        wordToObject[sentence].Add(obj.gameObject);


        if(wordSelected[sentence].selected)
        {
            print("spawning with circle");
            obj.ShowCircle(wordSelected[sentence].player.character.color);
        }
    }


    public void MakeGuess(int i)
    {

        players[i].StartWaving(wordbank[UnityEngine.Random.Range(0, 5)]);
        bubbles[i].speechBubble.SetActive(true);
        bubbles[i].bubbleText.text = players[i].guessWord;

        if (!wordSelected[players[i].guessWord].selected)
        {
            foreach (var item in wordToObject[players[i].guessWord])
            {
                item.GetComponent<movement>().ShowCircle(players[i].character.color);
            }
        }
        wordSelected[players[i].guessWord] = new WordSelection(true, players[i]);
    }

    public void MakeGuess(string word)
    {

        players[0].StartWaving(word);
        bubbles[0].speechBubble.SetActive(true);
        bubbles[0].bubbleText.text = players[0].guessWord;

        if (!wordSelected[players[0].guessWord].selected)
        {
            foreach (var item in wordToObject[players[0].guessWord])
            {
                item.GetComponent<movement>().ShowCircle(players[0].character.color);
            }
        }
        wordSelected[players[0].guessWord] = new WordSelection(true, players[0]);
    }

    public void redButtonClick()
    {

        Vector3 temp;
        float value1 = (float)0.5, value2 = (float)0.47, value3 = (float)1.0;


        if (redButtonIsPushed == false)
        {
            keywordRecognizer.Start();
            //Saving start time of when the red button is clicked
            voiceActive = true;
            startTime = Time.time;

            //showAnswerTextbox();

            //make button disappear once clicked
            redButton.transform.localScale = new Vector3(0, 0, 0);
            showMicIcon();
            redButtonIsPushed = true;


            /*Rect rect1 = new Rect(value1, value1, value3, value3);
            redButton.image.overrideSprite = redButton1;
            redButton.transform.GetComponent<RectTransform>().rect.Set(value1, value1, value1, value1);*/

        }
        /*else
        {
            
            hideAnswerTextbox();
            redButton.transform.localScale = new Vector3(1, 1, 1);
            redButtonIsPushed = false;

            /*Rect rect2 = new Rect(value2, value2, value3, value3);
            redButton.image.overrideSprite = redButton2;
            redButton.transform.GetComponent<RectTransform>().rect.Set(value2, value2, value2, value2);
        } */
    }

    public void showAnswerTextbox()
    {
        InputFieldAnswer.transform.localScale = new Vector3((float)0.44603, (float)0.44603, (float)0.44603);
        confirmTextButton.transform.localScale = new Vector3((float)0.44603, (float)0.44603, (float)0.44603);

    }

    public void hideAnswerTextbox()
    {
        InputFieldAnswer.transform.localScale = new Vector3(0, 0, 0);
        confirmTextButton.transform.localScale = new Vector3(0, 0, 0);
    }

    public void confirmButtonPushed()
    {
        hideAnswerTextbox();
        redButton.transform.localScale = new Vector3((float)0.22301, (float)0.22301, (float)0.22301);
        redButtonIsPushed = false;
    }

    public void showMicIcon()
    {
        micIcon.transform.localScale = new Vector3((float)0.07628, (float)0.064, (float)0.44603);
    }

    public void hideMicIcon()
    {
        micIcon.transform.localScale = new Vector3(0, 0, 0);
    }


    public void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();

        //If speech recognized, show textbox input
        showAnswerTextbox();
        hideMicIcon();
        keywordRecognizer.Stop();

    }

    public void Brush() { }
    public void Dry() { }
    public void Tie() { }
    public void Wash() { }
    public void Wear() { }

    public void DontBrush() { }
    public void DontDry() { }
    public void DontTie() { }
    public void DontWash() { }
    public void DontWear() { }

    public void DoNotBrush() { }
    public void DoNotDry() { }
    public void DoNotTie() { }
    public void DoNotWash() { }
    public void DoNotWear() { }


}
