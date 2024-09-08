﻿using UnityEngine;

public abstract class PlayerMovementBase : MonoBehaviour
{
    protected GroundDetection _detector;
    protected Rigidbody2D rb;
    protected Vector2 _velocityVector;
    protected PlayerRun _playerRun;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _detector = GetComponent<GroundDetection>();
        _playerRun = GetComponent<PlayerRun>();
    }

    protected void SetVelocity(float? x = null, float? y = null)
    {
        _velocityVector = rb.velocity;
        _velocityVector.x = x ?? _velocityVector.x;
        _velocityVector.y = y ?? _velocityVector.y;
        rb.velocity = _velocityVector;
    }
}