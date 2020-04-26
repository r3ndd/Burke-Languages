using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform camera;
    public Transform target;
    public Transform location;
    private float smoothSpeed = .05f;

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
