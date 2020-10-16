using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Language : MonoBehaviour
{
    public string name;
    public LinkedList<Topic> lessons;

    public Language(string nm)
    {
        name = nm;
    }
}

public class Topic
{
    public string name;
    public int language;
    public Scene game;
    private Vocab[] contents;

    public Topic(string nm, int lang)
    {
        name = nm;
        language = lang;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(game.name, LoadSceneMode.Single);
    }
}

public class Vocab
{
    private string question;
    private string answer;
    //image
    //part of a scene
}
