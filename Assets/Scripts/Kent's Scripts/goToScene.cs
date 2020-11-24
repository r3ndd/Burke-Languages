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

    public void toPP_Kitchen()
    {
        SceneManager.LoadScene("PP_Kitchen", LoadSceneMode.Single);
    }
}
