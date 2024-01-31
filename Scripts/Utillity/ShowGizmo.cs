using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmo : MonoBehaviour {
    public Color _color = Color.green;  //  default color
    public float _radius = 3f;  //  default radius

    void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
