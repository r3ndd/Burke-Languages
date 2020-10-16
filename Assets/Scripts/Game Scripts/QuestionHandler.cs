using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionHandler : MonoBehaviour
{
    public Question[] parentQuestions;
    public Question currentQuestion;
    public int numOfQuestionsToAsk;
    //public int nativeLanguage;
    public int targetLanguage;
    System.Random rng = new System.Random();

    public QuestionHandler(Question[] questions, int gameLen)
    {
        parentQuestions = questions;
        currentQuestion = parentQuestions[rng.Next(parentQuestions.Length)];
        numOfQuestionsToAsk = gameLen;
        targetLanguage = PlayerPrefs.GetInt("tLanguage", 1);
    }

    public void NextQuestion()
    {
        numOfQuestionsToAsk--;
        currentQuestion = currentQuestion.GetNext();
        if(currentQuestion == null)
        {
            currentQuestion = parentQuestions[rng.Next(parentQuestions.Length)];
        }
    }


}

public class Question //this is intended to encapsulate a single question/translation
{
    string[] questionText;//this array contains the translations in different languages and should always be in the following order
    //english, spanish, ... tbd
    public ViewData view;
    private Question next;

    public Question(string[] texts, ViewData lookingInfo, Question nex)
    {
        questionText = texts;
        view = lookingInfo;
        next = nex;
    }

    public string GetText(int languageindex)
    {
        return questionText[languageindex];
    }

    public ViewData GetTarget()
    {
        return view;
    }

    public Question GetNext()
    {
        return next;
    }
}

