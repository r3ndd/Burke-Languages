using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void ToPP_Game()
    {
        SceneManager.LoadScene("PP_Game", LoadSceneMode.Single);
    }

    public void ToVocabReview()
    {
        SceneManager.LoadScene("VocabReview", LoadSceneMode.Single);
    }
}
