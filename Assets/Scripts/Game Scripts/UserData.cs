using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    //enum languages { "English", "Spanish", "French"};

    public int nativeLanguage;
    public int targetLanguage;
    //public int points;
    //public float volume;
    // Start is called before the first frame update
    void Start()
    {
        nativeLanguage = PlayerPrefs.GetInt("nLanguage");
        targetLanguage = PlayerPrefs.GetInt("language");
        //volume = PlayerPrefs.GetFloat("volume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
