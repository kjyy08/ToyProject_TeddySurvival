using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    public Transform target;
    public float dist = 3.0f;
    public float heigth = 5.0f;
    public float width = 0.0f;
    public float smoothRotate = 5.0f;
    
    // Update is called once per frame
    void LateUpdate()
    {
        float yAngle = Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);
        Quaternion rotation = Quaternion.Euler(0f, yAngle, 0f);
        
        transform.position = target.position - (Vector3.forward * dist) + (Vector3.up * heigth) + (Vector3.right * width);
        transform.LookAt(target);
    }
}
