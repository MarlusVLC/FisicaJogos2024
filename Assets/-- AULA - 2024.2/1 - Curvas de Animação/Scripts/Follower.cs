using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private AnimationCurve customCurve;


    private void Update()
    {
        if (GetDistance() > 0.1f)
        {
            
        }
    }

    private float GetDistance()
    {
        return Vector2.Distance(transform.position, target.position);
    }
}
