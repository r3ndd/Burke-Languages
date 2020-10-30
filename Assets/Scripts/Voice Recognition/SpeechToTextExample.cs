using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechToTextExample : MonoBehaviour
{

    private Text textObj;
    private DictationRecognizer dictRecog;
    private bool listening = false;

    private void Start()
    {
        textObj = transform.GetChild(0).GetComponent<Text>();
        dictRecog = new DictationRecognizer();

        dictRecog.DictationResult += (text, confidence) =>
        {
            textObj.text = text;
            listening = false;
            dictRecog.Stop();
        };
    }

    public void OnClick()
    {
        if (!listening)
        {
            listening = true;
            textObj.text = "Listening...";
            dictRecog.Start();
        }
        else
        {
            listening = false;
            dictRecog.Stop();
        }
    }

}
