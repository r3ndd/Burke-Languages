using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToScene : MonoBehaviour
{
    public void toMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void toPP_Game()
    {
        SceneManager.LoadScene("PP_Game", LoadSceneMode.Single);
    }

    public void toVocabReview()
    {
        SceneManager.LoadScene("VocabReview", LoadSceneMode.Single);
    }
}
