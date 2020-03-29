using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Language : MonoBehaviour
{
    public string name;
    public LinkedList<topic> lessons;

    public Language(string nm)
    {
        name = nm;
    }
}

public class topic
{
    public string name;
    public int language;
    public Scene game;
    private vocab[] contents;

    public topic(string nm, int lang)
    {
        name = nm;
        language = lang;
    }

    public void startGame()
    {
        SceneManager.LoadScene(game.name, LoadSceneMode.Single);
    }
}

public class vocab
{
    private string question;
    private string answer;
    //image
    //part of a scene
}
