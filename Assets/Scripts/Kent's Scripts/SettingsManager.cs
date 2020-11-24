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
        gramDrop.value = PlayerPrefs.GetInt("responseType");
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
        PlayerPrefs.SetInt("responseType", grammar);
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

    public void setGame0()
    {
        PlayerPrefs.SetInt("gameType", 0);
    }

    public void setGame1()
    {
        PlayerPrefs.SetInt("gameType", 1);
    }

    public void setDif1()
    {
        PlayerPrefs.SetInt("difficulty", 1);
    }
    public void setDif2()
    {
        PlayerPrefs.SetInt("difficulty", 2);
    }
    public void setDif3()
    {
        PlayerPrefs.SetInt("difficulty", 3);
    }
    public void setDif4()
    {
        PlayerPrefs.SetInt("difficulty", 4);
    }
}
