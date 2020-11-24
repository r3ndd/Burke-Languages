using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform camera;
    public Transform target;
    public Transform location;
    private float smoothSpeed = .05f;
    public GameObject gameGUI;
    public GameObject vocabGUI;

    void Start()
    {
        if(PlayerPrefs.GetInt("gameType") == 0)
        {
            vocabGUI.SetActive(true);
            gameGUI.SetActive(false);
        }
        else
        {
            vocabGUI.SetActive(false);
            gameGUI.SetActive(true);
        }
    }

    public void changeView(viewData newView)
    {
        target = newView.target;
        location = newView.location;
    }
    void LateUpdate()
    {
        camera.LookAt(target);
        Vector3 nextPosition = Vector3.Lerp(camera.position, location.position, smoothSpeed);
        camera.position = nextPosition;
    }
}
