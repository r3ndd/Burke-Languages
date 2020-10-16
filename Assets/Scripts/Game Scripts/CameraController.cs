using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform location;
    private float smoothSpeed = .05f;

    public void ChangeView(ViewData newView)
    {
        target = newView.target;
        location = newView.location;
    }
    void LateUpdate()
    {
        transform.LookAt(target);
        transform.position = Vector3.Lerp(transform.position, location.position, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, location.rotation, smoothSpeed * Time.deltaTime);
    }
}
