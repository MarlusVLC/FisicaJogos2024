using System;
using _6.AcaoReacao;
using UnityEngine;

public class PlayerWalk : PlayerMovementBase
{
    [field: SerializeField] public MultipliableFloat MaxSpeed { get; private set; }
    [field: SerializeField] public MultipliableFloat Acceleration { get; private set; }
    [field: SerializeField] public MultipliableFloat Deceleration { get; private set; }
    [field: SerializeField] public MultipliableFloat TurnSpeed { get; private set; }
    [SerializeField] private bool useAcceleration = true;

    private float _targetSpeed;
    private float _speedChangeRate;

    private void Start()
    {
        TargetInput.Instance.onDirectionalAxisPressed.AddListener(SetMovementDirection);
    }

    public void FixedUpdate()
    {
        if (useAcceleration)
        {
            Accelerate();
        }
        else
        {
            MoveLinearly();
        } 
    }

    public void SetMovementDirection(Vector3 direction)
    {
        TryAlignScale(direction.x);
        _targetSpeed = direction.x * MaxSpeed;
    }

    public void TryAlignScale(float direction)
    {
        if (direction == 0)
            return;
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
        transform.localScale = scale;
    }

    public void MoveLinearly()
    {
        SetVelocity(x: _targetSpeed);
    }

    public void Accelerate()
    {
        _velocityVector = rb.velocity;

        if (_velocityVector.x == 0)
        {
            if (_targetSpeed != 0)
            {
                _speedChangeRate = Acceleration;
            }
        }
        else
        {
            if (_targetSpeed == 0)
            {
                _speedChangeRate = Deceleration;
            }
            else if (Mathf.Sign(_velocityVector.x) == Mathf.Sign(_targetSpeed))
            {
                _speedChangeRate = Acceleration;
            }
            else
            {
                _speedChangeRate = TurnSpeed;
            }
        }
        
        _velocityVector.x = Mathf.MoveTowards(_velocityVector.x, _targetSpeed, _speedChangeRate * Time.fixedDeltaTime);
        rb.velocity = _velocityVector;
    }
}
