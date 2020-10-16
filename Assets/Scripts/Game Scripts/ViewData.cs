using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewData : MonoBehaviour
{
    public Transform target;
    public Transform location;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(target.position, location.position);
    }
}
