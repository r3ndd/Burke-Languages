using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionHandler : MonoBehaviour
{
    private Question[] questions;
    private Question currentQuestion;
    private int questionIndex;
    private int numOfQuestionsToAsk;
    private int targetLanguage;

    public QuestionHandler(string[][] questionStrings, GameObject[] targets)
    {
        questions = new Question[questionStrings.Length];
        questionIndex = 0;
        numOfQuestionsToAsk = questions.Length;
        targetLanguage = PlayerPrefs.GetInt("tLanguage", 1);

        for (int i = 0; i < questionStrings.Length; i++)
            questions[i] = new Question(questionStrings[i], targets[i], this);
    }

    public void NextQuestion()
    {
        GetQuestion().StopAnimation();

        numOfQuestionsToAsk--;
        questionIndex++;
    }

    public Question GetQuestion()
    {
        return questions[questionIndex];
    }

    public int GetLangIndex()
    {
        return targetLanguage;
    }

    public void SetLangIndex(int index)
    {
        targetLanguage = index;
    }

}

public class Question //this is intended to encapsulate a single question/translation
{
    private string[] questionText; //this array contains the translations in different languages and should always be in the following order: english, spanish, ... tbd
    private GameObject target;
    private Transform view;
    private QuestionHandler handler;

    public Question(string[] texts, GameObject _target, QuestionHandler _handler)
    {
        questionText = texts;
        target = _target;
        view = _target.transform.Find("View");
        handler = _handler;
    }

    public string GetText()
    {
        return questionText[handler.GetLangIndex()];
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public Transform GetView()
    {
        return view;
    }

    public void PlayAnimation()
    {
        target.SetActive(true);
    }

    public void StopAnimation()
    {
        target.SetActive(false);
    }
}

