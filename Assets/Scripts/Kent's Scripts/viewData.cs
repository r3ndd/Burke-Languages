using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewData : MonoBehaviour
{
    public Transform target;
    public Transform location;
    public GameObject anime;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(target.position, location.position);
    }

    public void toggleAnime(bool onOff)
    {
        Debug.Log("Setting animation to " + onOff);
        anime.SetActive(onOff);
    }
}
