using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class Game2Controller : MonoBehaviour
{

    public AudioSource rewardSoundSource;

    public static Game2Controller instance;

    public Transform arrow;

    public Transform[] positions;

    public int posTracker;

    public bool recognizedNewWord;
    public string wordRecognized;

    public Dictionary<string, Vector3> wordToPosition = new Dictionary<string, Vector3>();


    public TextMeshPro timerText;
    public float timer = 60f;


    public TextMeshPro addPointsPrefab;


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

    public SpriteRenderer speechBubble;
    public Sprite middleBubble, sideBubble;
    public TextMeshPro bubbleText;

    enum Game2State { Demand, Negate, Phrase, Correct, Wrong}

    public SpriteRenderer iconImage;
    public GameObject noSign;
    float iconImageAlpha;

    public float iconFadeSpeed;

    Game2State gameState;
    Game2State phraseState;

    public Text phraseText;

    float phraseTimer;
    public float phraseTime;

    bool newState;

    bool canGuess;

    public string phrase = "";

    public List<ActionObject> listOfActions;

    int i;

    bool gameOver = false;


    public List<CharacterObject> listOfCharacters;

    public List<Game1Player> players;

    bool gameRunning;

    Game1Player personGuessing;

    public AudioSource endGame;

    public TextMeshPro startText;

    //public GameObject addScorePrefab;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
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


        if (MenuSelection.instance)
        {
            MenuSelection.instance.FadeIn(1f);
        }


        if (IntroController.instance)
        {
            listOfCharacters = IntroController.instance.charactersInGame;
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.musicSource.clip = AudioManager.instance.game1;
            AudioManager.instance.musicSource.Play();
        }

        for (int i = 0; i < 3; i++)
        {
            print("here");
            players[i].character = listOfCharacters[i];
            players[i].ChangeSprite();
        }


        iconImageAlpha = 0f;
        iconImage.color = new Color(1f, 1f, 1f, 0f);
        ActivateGameObject(noSign, false);

        gameState = Game2State.Demand;
        newState = true;
        canGuess = true;

        i = 1;

        speechBubble.gameObject.SetActive(false);

        ActivateGameObject(startText.gameObject, true);

        InitGame();
    }


    private void Update()
    {
        if (gameRunning)
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
            

                if (recognizedNewWord)
                {
                    recognizedNewWord = false;

                    if ((gameState == Game2State.Demand || gameState == Game2State.Negate))
                    {
                        ActivateGameObject(phraseText.gameObject, true);
                        SetBubbles(1);

                        gameState = Game2State.Phrase;
                        phraseText.text = wordRecognized;
                        phraseTimer = 0f;
                        newState = true;

                    }
                }


                if (gameRunning)
                {
                //check if voice input has been activated i.e. red button is clicked.
                if (redButtonIsPushed == false)
                {
                    redButton.transform.GetComponent<RectTransform>().rect.Set(value1, value1, value1, value1);
                    if ((gameState == Game2State.Demand || gameState == Game2State.Negate))
                    {
                        if (Time.frameCount % 60 == 0)
                        {
                            for (int i = 1; i < 3; i++)
                            {

                                if (UnityEngine.Random.value <= 0.155)
                                {
                                    SetBubbles(i + 1);

                                    gameState = Game2State.Phrase;

                                    phraseTimer = 0f;
                                    newState = true;
                                }
                            }
                        }
                    }
                }


                if (timer > 0f)
                {
                    timer -= Time.deltaTime;
                    timerText.text = Mathf.CeilToInt(timer).ToString();
                }
                else
                {
                    if (!gameOver)
                    {
                        gameOver = true;
                        EndGame();
                    }
                }

                #region DEMAND
                if (gameState == Game2State.Demand)
                {
                    if (newState)
                    {
                        phraseState = Game2State.Demand;
                        iconImage.color = new Color(1f, 1f, 1f, 0f);
                        iconImageAlpha = 0f;
                        iconImage.GetComponent<ActionIcon>().Init(listOfActions[UnityEngine.Random.Range(0, listOfActions.Count)]);
                        phrase = iconImage.GetComponent<ActionIcon>().action.commandSentence;
                        newState = false;

                        speechBubble.gameObject.SetActive(false);
                    }

                    if (iconImage.color.a != 1f)
                    {
                        iconImageAlpha += Time.deltaTime / iconFadeSpeed;
                        iconImage.color = new Color(1f, 1f, 1f, iconImageAlpha);

                    }
                }
                #endregion
                #region PHRASE
                else if (gameState == Game2State.Phrase)
                {
                    if (newState)
                    {
                        phraseTimer = 0f;
                        phraseTime = 1.5f;
                        newState = false;
                    }

                    if (phraseTimer <= phraseTime)
                        phraseTimer += Time.deltaTime;
                    else
                    {
                        if (wordRecognized == phrase && canGuess)
                        {
                            newState = true;
                            gameState = Game2State.Correct;
                        }
                        else
                        {
                            //newState = true;
                            gameState = phraseState;
                            SetBubbles(0);
                        }
                    }
                }
                #endregion
                #region CORRECT
                else if (gameState == Game2State.Correct)
                {
                    if (newState)
                    {
                        newState = false;

                        print("correct!");
                        PlayRewardSound();
                        phraseTimer = 0f;
                        phraseTime = 1f;
                        FullAlpha();
                        personGuessing.UpdateScore(10);
                        var addPoints = Instantiate(addPointsPrefab, personGuessing.scoreText.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                        addPoints.color = personGuessing.character.color;
                        if (phraseState == Game2State.Negate)
                        {
                            ActivateGameObject(noSign, false);
                            iconImage.GetComponent<ActionAnimate>().animate = false;
                        }
                    }

                    if (phraseTimer <= phraseTime)
                        phraseTimer += Time.deltaTime;

                    else
                    {
                        phraseTimer = 0f;

                        SetBubbles(0);

                        newState = true;
                        gameState = phraseState == Game2State.Demand ? Game2State.Negate : Game2State.Demand;
                        iconImage.GetComponent<ActionAnimate>().animate = phraseState == Game2State.Demand ? true : false;

                    }

                }
                #endregion
                #region NEGATE
                else if (gameState == Game2State.Negate)
                {
                    if (newState)
                    {
                        phraseState = Game2State.Negate;
                        phrase = iconImage.GetComponent<ActionIcon>().action.negateSentence;
                        newState = false;
                        canGuess = false;
                    }

                    if (phraseTimer <= phraseTime)
                        phraseTimer += Time.deltaTime;
                    else
                    {
                        ActivateGameObject(noSign, true);
                        canGuess = true;
                    }

                }
                #endregion


                if (Input.GetKeyDown(KeyCode.Z))
                {
                    MakeGuess(1);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {


                    MakeGuess(2);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {

                    MakeGuess(3);
                }


                if (Input.GetKeyDown(KeyCode.V))
                {
                    AIGuess(1);
                }


                if (Input.GetKeyDown(KeyCode.P))
                {
                    timer = 2f;
                }
            }
            else
            {
                redButton.transform.GetComponent<RectTransform>().rect.Set(value2, value2, value2, value2);
            }

        }
    }

    public void SetTImer(float time)
    {
        timer = time;
    }

    void AIGuess(int i)
    {
        if ((gameState == Game2State.Demand || gameState == Game2State.Negate) && canGuess)
        {
            recognizedNewWord = false;

            wordRecognized = phrase;
            ActivateGameObject(phraseText.gameObject, true);
            SetBubbles(i+1);

            gameState = Game2State.Phrase;
            phraseText.text = wordRecognized;
            phraseTimer = 0f;
            newState = true;
            personGuessing = players[i];
        }
    }


    void EndGame()
    {
        if (gameRunning)
        {
            gameRunning = false;
            MenuSelection.instance.GoToMenuScene(3f, MenuSelection.instance.selectedMinigame.sceneName);
            print(MenuSelection.instance.selectedMinigame.sceneName);
            ActivateGameObject(startText.gameObject, true);
            startText.text = "Finish!";
            MenuSelection.instance.selectedMinigame.Scores.Add(players[0].score);
            endGame.Play();
        }
        //StaticVariables.minigame.Scores.Add(points[0]);
        //SceneManager.LoadScene(menuScene.name);
    }



    void PlayRewardSound()
    {
        rewardSoundSource.Play();
    }

    void FullAlpha()
    {
        iconImage.color = new Color(1f, 1f, 1f, 1f);
    }

    void ActivateGameObject(GameObject go, bool active)
    {
        go.SetActive(active);
    }

    void SetBubbles(int i)
    {
        if (i == 0)
        {
            ActivateGameObject(speechBubble.gameObject, false);
        }
        else if (i == 1)
        {
            ActivateGameObject(speechBubble.gameObject, true);
            speechBubble.sprite = sideBubble;
            speechBubble.flipX = false;
        }
        else if (i == 2)
        {
            ActivateGameObject(speechBubble.gameObject, true);
            speechBubble.sprite = middleBubble;
        }
        else if (i == 3)
        {
            ActivateGameObject(speechBubble.gameObject, true);
            speechBubble.sprite = sideBubble;
            speechBubble.flipX = true;
        }

        if (i != 0)
        {
            wordRecognized = phrase;
            personGuessing = players[i - 1];
            bubbleText.text = wordRecognized;
        }

    }

    void SetBubbles(string word)
    {

        ActivateGameObject(speechBubble.gameObject, true);
        speechBubble.sprite = sideBubble;
        speechBubble.flipX = false;
        

        wordRecognized = word;
        personGuessing = players[0];
        bubbleText.text = wordRecognized;

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
        float duration = 2f;
        // keep adjusting the position while there is time
        while (journey <= duration)
        {
            // add to timer
            journey = journey + Time.deltaTime;
            yield return null;
        }
        #endregion

        gameRunning = true;
        ActivateGameObject(startText.gameObject, false);

    }

    public void MakeGuess(int i)
    {
        if (gameState == Game2State.Demand || gameState == Game2State.Negate)
        {

            SetBubbles(i);

            gameState = Game2State.Phrase;

            phraseTimer = 0f;
            newState = true;

        }
    }

    public void MakeGuess(string word)
    {
        if (gameState == Game2State.Demand || gameState == Game2State.Negate)
        {

            SetBubbles(word);

            gameState = Game2State.Phrase;

            phraseTimer = 0f;
            newState = true;

        }
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
