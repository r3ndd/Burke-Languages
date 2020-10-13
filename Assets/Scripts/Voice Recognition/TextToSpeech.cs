using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class TextToSpeech : MonoBehaviour
{
    // Larger -> more lenient
    public float accuracyLeniency = 3.0f;

    private DictationRecognizer dictRecog;
    private bool listening = false;
    private Action<string> onResult;

    private void Start()
    {
        dictRecog = new DictationRecognizer();

        dictRecog.DictationResult += (text, confidence) =>
        {
            listening = false;
            dictRecog.Stop();
            onResult(text);
        };
    }

    public void GetSpeech(Action<string> callback)
    {
        if (!listening)
        {
            listening = true;
            onResult = callback;
            dictRecog.Start();
        }
    }

    public void ListenAndScore(string target, Action<float> callback)
    {
        GetSpeech((string text) => {
            callback(GetTextAccuracy(text, target));
        });
    }

    public void ListenAndScore(string[] targets, Action<float> callback)
    {
        GetSpeech((string text) => {
            callback(GetTextAccuracy(text, targets));
        });
    }

    public float GetTextAccuracy(string input, string target)
    {
        input = input.ToLower();
        target = target.ToLower();

        float scoreTotal = 0.0f;

        for (int i = 0; i < target.Length; i++)
        {
            int dist = 0;
            int leftIndex = i;
            int rightIndex = i;
            bool matchedChar;

            while (leftIndex > 0 || rightIndex < input.Length)
            {
                matchedChar = false;

                if (leftIndex > 0 && input[leftIndex] == target[i])
                    matchedChar = true;
                else if (rightIndex < input.Length && input[rightIndex] == target[i])
                    matchedChar = true;

                if (matchedChar)
                {
                    scoreTotal += (float)Math.Exp(-1 * dist / accuracyLeniency);
                    break;
                }

                dist++;
                leftIndex--;
                rightIndex++;
            }
        }

        return scoreTotal / target.Length;
    }

    public float GetTextAccuracy(string input, string[] targets)
    {
        float highestScore = 0.0f;

        for (int i = 0; i < targets.Length; i++)
        {
            float score = GetTextAccuracy(input, targets[i]);

            if (score > highestScore)
                highestScore = score;
        }

        return highestScore;
    }


}
