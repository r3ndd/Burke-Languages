using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform camera;
    public float lerpSpeed = 1.0f;
    public float startAnimDist = 0.1f;
    public GameObject[] animations;

    private TextToSpeech textToSpeech;
    private int questionIndex;
    private GameObject targetAnimation;
    private Transform targetCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        textToSpeech = GetComponent<TextToSpeech>();

        GoToAnimation(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetAnimation)
            return;

        camera.position = Vector3.Lerp(camera.position, targetCameraPos.position, lerpSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Lerp(camera.rotation, targetCameraPos.rotation, lerpSpeed * Time.deltaTime);

        if (Vector3.Distance(camera.position, targetCameraPos.position) < startAnimDist)
        {
            targetAnimation.SetActive(true);

            switch (questionIndex) {
                case 0:
                    textToSpeech.ListenAndScore("hola como estas", (score) => 
                    {
                        // Check score and ask again or go to next question
                    });
                    break;
            }
        }
    }

    void GoToAnimation(int index)
    {
        if (targetAnimation)
            targetAnimation.SetActive(false);

        questionIndex = index;
        targetAnimation = animations[index];
        targetCameraPos = targetAnimation.transform.Find("Camera Pos").transform;
    }
}
