using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionHandler : MonoBehaviour
{
    public question[] parentQuestions;
    public question currentQuestion;
    public int numOfQuestionsToAsk;
    //public int nativeLanguage;
    public int targetLanguage;
    System.Random rng = new System.Random();

    public questionHandler(question[] questions, int gameLen)
    {
        parentQuestions = questions;
        currentQuestion = parentQuestions[rng.Next(parentQuestions.Length)];
        numOfQuestionsToAsk = gameLen;
        targetLanguage = PlayerPrefs.GetInt("tLanguage", 1);
    }

    public void nextQuestion()
    {
        numOfQuestionsToAsk--;
        currentQuestion = currentQuestion.getNext();
        if(currentQuestion == null)
        {
            currentQuestion = parentQuestions[rng.Next(parentQuestions.Length)];
        }
    }


}

public class question //this is intended to encapsulate a single question/translation
{
    string[] questionText;//this array contains the translations in different languages and should always be in the following order
    //english, spanish, ... tbd
    public viewData view;
    private question next;

    public question(string[] texts, viewData lookingInfo, question nex)
    {
        questionText = texts;
        view = lookingInfo;
        next = nex;
    }

    public string getText(int languageindex)
    {
        return questionText[languageindex];
    }

    public viewData getTarget()
    {
        return view;
    }

    public question getNext()
    {
        return next;
    }
}

