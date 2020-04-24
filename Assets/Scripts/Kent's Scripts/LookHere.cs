using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookHere : MonoBehaviour
{
    public Transform cameraLocation;
    public Transform cameraTarget;
    public string[] answers;
    public Transform sam;
    public float smoothSpeed = .125f;
    [SerializeField]
    private LookHere[] lookHeres;

    public bool checkAnswer(string guess, int languageIndex)
    {
        if (answers[languageIndex].ToLower().Replace(".", "") == guess.ToLower().Replace(".", "")) return true;
        return false;
    }

    public void lookAtMe(Transform cam)
    {
        sam = cam;
    }

    void LateUpdate()
    {
        sam.LookAt(cameraTarget);
        Vector3 nextPosition = Vector3.Lerp(sam.position, cameraLocation.position, smoothSpeed);
        sam.position = nextPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cameraLocation.position, cameraTarget.position);
    }
}
