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

    public void SaveLanguage(int language)
    {
        PlayerPrefs.SetInt("tLanguage", language);
    }

    public void SaveGrammar(int grammar)
    {
        PlayerPrefs.SetInt("grammar", grammar);
    }

    public void SaveVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume", vol);
        mixer.SetFloat("volume", vol);
    }
    
    public void SaveAuthentication()
    {
        Debug.Log("Saved");
        PlayerPrefs.SetInt("Authentication", signedIn);
    }

    public void Suthenticate()
    {
        Debug.Log("Signed On");
        signedIn = 1;
        SaveAuthentication();
    }

    public void SignOut()
    {
        Debug.Log("Signed off");
        signedIn = 0;
        SaveAuthentication();
    }
}
