using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    public float speed = 1f;
    
    void Update()
    {
        transform.Translate(
            Input.GetAxis("Horizontal") * speed, 
            Input.GetAxis("Vertical") * speed, 
            Input.GetAxis("Depth") * speed);
    }
}
