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
    }


}

public class question //this is intended to encapsulate a single question/translation
{
    string[] questionText;//this array contains the translations in different languages and should always be in the following order
    //english, spanish, ... tbd
    public int viewIndex;
    private question next;

    public question(string[] texts, int viewInt, question nex)
    {
        questionText = texts;
        viewIndex = viewInt;
        next = nex;
    }

    public string getText(int languageindex)
    {
        return questionText[languageindex];
    }

    public int getTarget()
    {
        return viewIndex;
    }

    public question getNext()
    {
        return next;
    }
}

