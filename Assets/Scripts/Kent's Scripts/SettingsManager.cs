using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    public TMPro.TMP_Dropdown langDrop;
    public TMPro.TMP_Dropdown gramDrop;
    public Slider volumeSlider;
    public AudioMixer mixer;

    private void Start()
    {
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        langDrop.value = PlayerPrefs.GetInt("tLanguage");
        gramDrop.value = PlayerPrefs.GetInt("grammar");
        volumeSlider.value = -10f;// PlayerPrefs.GetFloat("volume");
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
}
