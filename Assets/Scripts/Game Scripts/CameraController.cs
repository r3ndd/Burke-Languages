using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform view;
    public float smoothSpeed = .05f;

    public void ChangeView(Transform newView)
    {
        view = newView;
    }
    void LateUpdate()
    {
        if (view == null)
            return;

        transform.position = Vector3.Lerp(transform.position, view.position, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, view.rotation, smoothSpeed * Time.deltaTime);
    }
}
