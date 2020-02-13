using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

[RequireComponent(typeof(Button))]
public class Game3Controller : MonoBehaviour
{

    public static Game3Controller instance;

    public enum Game3State { Choosing, ChoosingPause, DisplayAnswer}

    public SpriteRenderer action;

    float timer;
    public TextMeshPro timerText;

    public Game3State state;

    public bool gameStart = false;

    public Game1Player[] players;



    //public int amtInLeft, amtInRight;

    public int amtInLeft
    {
        get
        {
            return leftPath.Count;
        }
    }
    public int amtInRight
    {
        get
        {
            return rightPath.Count;
        }
    }



    public Transform leftDoor, rightDoor;


    public int amtLeft;

    bool newState;


    public AnimationCurve movePlayersCurve;
    public float movePlayerSpeed;

    public bool redButtonIsPushed;
    public Button redButton;
    public Sprite redButton1;
    public Sprite redButton2;
    public Image micIcon;
    public float value1 = (float)0.5, value2 = (float)0.47, value3 = (float)1.0;
    public KeywordRecognizer keywordRecognizer;
    public Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public float startTime;
    public bool voiceActive;

    public InputField InputFieldAnswer;
    public Button confirmTextButton;

    public bool winningSide;
    public SpriteRenderer leftImage, rightImage;
    public Sprite correctImage, incorrectImage;
    public Transform checkmark;

    public Vector3[] startPositions;

    public TextMeshPro roundText;
    int roundNumber, maxRounds;

    bool endGame, gameRunning;

    public List<ActionObject> listOfNouns;
    public PoseToNoun correctObj;


    public List<PoseToNoun> poseToNouns;

    public List<Game1Player> leftPath, rightPath;

    public List<Sprite> answerPanels;
    public List<string> answerWords;
    public AudioSource endGameSource;

    public TextMeshPro startText;

    [System.Serializable]
    public struct PoseToNoun
    {
        public Sprite pose;
        public List<Sprite> correctAnswers;
        public List<string> correctWord;
    }

    public GameObject addPointsPrefab;

    public string correctWord, incorrectWord;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        voiceActive = false;

        //------------------- Start of Voice Recognition Section --------------------
        //Adding to dictionary
        //"forgot" answer/animation
        actions.Add("forgot", Forgot);
        //"Ran out of milk" answer/animation
        actions.Add("ran out of", RanOut);
        actions.Add("ran out", RanOut);
        actions.Add("ran", RanOut);
        //"Broke" answer/animation
        actions.Add("broke", Broke);
        //"Dropped" answer/animation
        actions.Add("dropped", Dropped);

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
        state = Game3State.Choosing;
        amtLeft = 4;
        newState = true;
        roundNumber = 1;
        maxRounds = 5;

        endGame = false;
        gameRunning = false;

        if (MenuSelection.instance)
        {
            MenuSelection.instance.FadeIn(1f);
        }


        if (IntroController.instance)
        {
            var listOfplayers = IntroController.instance.charactersInGame;

            for (int i = 0; i < listOfplayers.Count; i++)
            {
                players[i].character = listOfplayers[i];
                players[i].ChangeSprite();
            }
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.musicSource.clip = AudioManager.instance.game1;
            AudioManager.instance.musicSource.Play();
        }

        startPositions = new Vector3[4];
        foreach (var item in players)
        {
            startPositions[item.i] = item.transform.position;
        }

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
            //check if voice input has been activated i.e. red button is clicked.
            if (redButtonIsPushed == false)
            {

                redButton.transform.GetComponent<RectTransform>().rect.Set(value1, value1, value1, value1);
                if (Input.GetKeyDown(KeyCode.P))
                {
                    roundNumber = 6;
                }


                if (state == Game3State.Choosing)
                {
                    if (newState)
                    {
                        newState = false;
                        timer = 10f;


                        leftPath.Clear();
                        rightPath.Clear();
                        amtLeft = 4;

                        foreach (var item in players)
                        {
                            item.guess = false;
                        }

                        PlaceImages();

                        roundText.text = "Round " + roundNumber;

                        PlacePlayers();

                    }

                    UpdateTimer();



                    for (int i = 1; i < players.Length; i++)
                    {
                        if (!players[i].guess)
                        {
                            var randomChance = UnityEngine.Random.value;
                            if (randomChance < 0.005f)
                            {
                                var randomSide = UnityEngine.Random.value < 0.5f ? true : false;

                                if (randomSide)
                                {
                                    players[i].guess = true;

                                    StartCoroutine(MoveCharacters(players[i], leftDoor.position + Vector3.right * (amtInLeft * 0.45f)));

                                    //players[i].transform.position = leftDoor.position + Vector3.right * (amtInLeft * 0.45f);
                                    //players[i].transform.localScale = Vector3.one * 0.35f;
                                    leftPath.Add(players[i]);

                                }
                                else
                                {

                                    players[i].guess = true;

                                    StartCoroutine(MoveCharacters(players[i], rightDoor.position + Vector3.left * (amtInRight * 0.45f)));

                                    //players[i].transform.position = rightDoor.position + Vector3.left * (amtInRight * 0.45f);
                                    //players[i].transform.localScale = Vector3.one * 0.35f;
                                    rightPath.Add(players[i]);

                                }
                                amtLeft--;
                            }
                        }
                    }

                    if (amtLeft == 0)
                    {
                        state = Game3State.ChoosingPause;
                        newState = true;
                        timerText.text = "";

                    }



                }
                else if (state == Game3State.ChoosingPause)
                {
                    if (newState)
                    {
                        newState = false;


                        timer = 2f;

                    }

                    timer -= Time.deltaTime;

                    if (timer <= 0f)
                    {
                        state = Game3State.DisplayAnswer;
                        newState = true;
                    }


                }
                else if (state == Game3State.DisplayAnswer)
                {
                    if (newState)
                    {
                        newState = false;
                        if (AudioManager.instance)
                            AudioManager.instance.PlayCorrectSound();
                        timer = 2f;

                        checkmark.gameObject.SetActive(true);
                        checkmark.transform.position = winningSide ? leftImage.transform.position : rightImage.transform.position;

                        foreach (var item in winningSide ? leftPath : rightPath)
                        {
                            item.UpdateScore(10);
                            var temp = Instantiate(addPointsPrefab, item.scoreText.transform.position, Quaternion.identity);
                            temp.transform.localScale = Vector3.one * 0.5f;
                            temp.GetComponent<TextMeshPro>().color = item.character.color;
                        }

                    }

                    timer -= Time.deltaTime;

                    if (timer <= 0f)
                    {
                        if (roundNumber > 4 && !endGame)
                        {
                            endGame = true;
                            print("ENDING GAME");
                            EndGame();
                        }

                        if (!endGame)
                        {
                            roundNumber++;
                            state = Game3State.Choosing;
                            newState = true;
                        }

                    }

                }
            }
            else
            {
                redButton.transform.GetComponent<RectTransform>().rect.Set(value2, value2, value2, value2);
            }
        }
    }

    public void SetRound(int round)
    {
        roundNumber = round;
    }

    void EndGame()
    {
        if (gameRunning)
        {
            gameRunning = false;
            MenuSelection.instance.GoToMenuScene(3f, MenuSelection.instance.selectedMinigame.sceneName);
            print(MenuSelection.instance.selectedMinigame.sceneName);
            startText.gameObject.SetActive(false);
            startText.text = "Finish!";
            MenuSelection.instance.selectedMinigame.Scores.Add(players[0].score);
            endGameSource.Play();
        }

    }


    void PlacePlayers()
    {
        foreach (var item in players)
        {
            item.transform.localScale = Vector3.one;
            item.transform.position = startPositions[item.i];
        }
    }




    void UpdateTimer()
    {
        timer -= Time.deltaTime;

        if(timer >= 0f)
            timerText.text = Mathf.CeilToInt(timer).ToString();
        else
        {
            timerText.text = "Time's Up!";
            state = Game3State.ChoosingPause;
            newState = true;


            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].guess)
                {
                    var randomSide = UnityEngine.Random.value < 0.5f ? true : false;

                    if (randomSide)
                    {
                        players[i].guess = true;

                        StartCoroutine(MoveCharacters(players[i], leftDoor.position + Vector3.right * (amtInLeft * 0.45f)));

                        //players[i].transform.position = leftDoor.position + Vector3.right * (amtInLeft * 0.45f);
                        //players[i].transform.localScale = Vector3.one * 0.35f;
                        leftPath.Add(players[i]);
                    }
                    else
                    {
                        players[i].guess = true;

                        StartCoroutine(MoveCharacters(players[i], rightDoor.position + Vector3.left * (amtInRight * 0.45f)));

                        //players[i].transform.position = rightDoor.position + Vector3.left * (amtInRight * 0.45f);
                        //players[i].transform.localScale = Vector3.one * 0.35f;
                        rightPath.Add(players[i]);
                    }
                    amtLeft--;
                }
            }
        }
    }


    public void MakeGuess(string word)
    {
        if (!string.Equals(word, correctWord) && !string.Equals(word, incorrectWord)) return;


        var choice = string.Equals(word, correctWord) ? winningSide : !winningSide;


        if (!players[0].guess)
        {
            Vector3 trans = choice ? leftDoor.position :rightDoor.position;
            var x = choice ? amtInLeft : amtInRight;
            players[0].guess = true;
            if (choice)
            {
                players[0].guess = true;

                StartCoroutine(MoveCharacters(players[0], leftDoor.position + Vector3.right * (amtInLeft * 0.45f)));
                leftPath.Add(players[0]);
            }
            else
            {
                players[0].guess = true;

                StartCoroutine(MoveCharacters(players[0], rightDoor.position + Vector3.left * (amtInRight * 0.45f)));

                rightPath.Add(players[0]);
            }
            amtLeft--;
        }
    }


    public IEnumerator MoveCharacters(Game1Player player, Vector3 dest)
    {

        Vector3 startingPos = player.transform.position;

        Vector3 endPos = dest;

        //Vector3 endPos = ????

        float moveTimer = 0f;
        float percent = 0f;

        while (moveTimer < movePlayerSpeed)
        {
            moveTimer += Time.deltaTime;

            percent = moveTimer / movePlayerSpeed;

            player.transform.position = Vector3.Lerp(startingPos, endPos, movePlayersCurve.Evaluate(percent));
            player.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.35f, movePlayersCurve.Evaluate(percent));

            yield return null;
        }


    }


    void PlaceImages()
    {
        winningSide = UnityEngine.Random.value < 0.5f ? true : false;

        correctObj = poseToNouns[UnityEngine.Random.Range(0, poseToNouns.Count)];


        var randIndexForCorrect = UnityEngine.Random.Range(0, correctObj.correctAnswers.Count);
        correctImage = correctObj.correctAnswers[randIndexForCorrect];


        var randIndexForIncorrect = UnityEngine.Random.Range(0, answerPanels.Count);
        incorrectImage = answerPanels[randIndexForIncorrect];


        for (int i = 0; i < correctObj.correctAnswers.Count; i++)
        {
            if (incorrectImage == correctImage)
            {
                incorrectImage = answerPanels[UnityEngine.Random.Range(0, answerPanels.Count)];
                i = 0;
            }
        }



        correctWord = correctObj.correctWord[correctObj.correctAnswers.IndexOf(correctImage)];
        incorrectWord = answerWords[answerPanels.IndexOf(incorrectImage)];


        leftImage.sprite = winningSide ? correctImage : incorrectImage;
        rightImage.sprite = winningSide ? incorrectImage : correctImage;

        checkmark.gameObject.SetActive(false);

        action.sprite = correctObj.pose;
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

        startText.gameObject.SetActive(false);
    }

    public void redButtonClick() {

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

    public void showAnswerTextbox() {
        InputFieldAnswer.transform.localScale = new Vector3(1, 1, 1);
        confirmTextButton.transform.localScale = new Vector3(1, 1, 1);
        
    }

    public void hideAnswerTextbox() {
        InputFieldAnswer.transform.localScale = new Vector3(0, 0, 0);
        confirmTextButton.transform.localScale = new Vector3(0, 0, 0);
    }

    public void confirmButtonPushed() {
        hideAnswerTextbox();
        redButton.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.5);
        redButtonIsPushed = false;
    }

    public void showMicIcon() {
        micIcon.transform.localScale = new Vector3((float)0.18, (float)0.14, (float)1);
    }

    public void hideMicIcon()
    {
        micIcon.transform.localScale = new Vector3(0, 0, 0);
    }


    public void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();

        //If speech recognized, show textbox input
        showAnswerTextbox();
        hideMicIcon();
        keywordRecognizer.Stop();
        
    }

 

    public void Forgot() { 
        
    }

    public void RanOut() { 
        
    }

    public void Broke() {
        
    }

    public void Dropped() {
        
    }

}
