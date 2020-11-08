using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    public TMPro.TMP_Dropdown langDrop;
    public TMPro.TMP_Dropdown gramDrop;
    public Slider volumeSlider;
    public AudioMixer mixer;
    public int signedIn = 0;
    public GameObject logIn;
    public GameObject overLay;
    public GameObject menu;

    private void Start()
    {
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        langDrop.value = PlayerPrefs.GetInt("tLanguage");
        gramDrop.value = PlayerPrefs.GetInt("grammar");
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        signedIn = PlayerPrefs.GetInt("Authentication");
        Debug.Log("Started");
        if (signedIn == 1)
        {
            Debug.Log("Did it");
            logIn.SetActive(false);
            overLay.SetActive(true);
            menu.SetActive(true);
        }
    }

    public void saveLanguage(int language)
    {
        PlayerPrefs.SetInt("tLanguage", language);
    }

    public void saveGrammar(int grammar)
    {
        PlayerPrefs.SetInt("grammar", grammar);
    }

    public void saveVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume", vol);
        mixer.SetFloat("volume", vol);
    }

    public void saveAuthentication()
    {
        Debug.Log("Saved");
        PlayerPrefs.SetInt("Authentication", signedIn);
    }

    public void authenticate()
    {
        Debug.Log("Signed On");
        signedIn = 1;
        saveAuthentication();
    }

    public void signOut()
    {
        Debug.Log("Signed off");
        signedIn = 0;
        saveAuthentication();
    }

    public void diff1()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
    }

    public void diff2()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
    }

    public void diff3()
    {
        PlayerPrefs.SetInt("Difficulty", 3);
    }

    public void diff4()
    {
        PlayerPrefs.SetInt("Difficulty", 4);
    }

    public int getDiff()
    {
        return PlayerPrefs.GetInt("Difficulty");
    }

    //The next to methods should be called by the buttons for playing the game or the vocab review.
    //These values will be retreived in the next scene and the appropriate GUI will be activated based on this.
    public void setToPlay()
    {
        PlayerPrefs.SetInt("Play", 1);
    }

    public void setToReview()
    {
        PlayerPrefs.SetInt("Play", 0);
    }

    public int getPlayValue()
    {
        return PlayerPrefs.GetInt("Play");
    }

}
