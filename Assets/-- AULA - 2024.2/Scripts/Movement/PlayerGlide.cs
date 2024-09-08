using System;
using _6.AcaoReacao;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGlide : PlayerMovementBase
{
    [Min(0.001f)][SerializeField] private float gravityDivisor;
    [SerializeField] private float maxFallSpeed;
    [field: SerializeField] public float DecelerationMultiplier { get; private set; }
    [field: SerializeField] public float MaxSpeedMultiplier { get; private set; }
    [field: SerializeField] public UnityEvent<bool> OnGlideToggle { get; private set; }

    public bool IsGliding { get; private set; }
    
    private bool _isGlidingIntended;

    private bool IsFalling => rb.velocity.y < 0f;
    

    private void Start()
    {
        TargetInput.Instance.onControlPressed.AddListener(() => _isGlidingIntended = true);
        TargetInput.Instance.onControlReleased.AddListener(() => _isGlidingIntended = false);
        _detector.AddCallbackOnGroundDetection(() => _isGlidingIntended = false, true);
    }

    private void FixedUpdate()
    {
        if (!IsGliding) 
            return;
        if (Mathf.Abs(rb.velocity.y) > maxFallSpeed)
        {
            SetVelocity(y: -maxFallSpeed);
        }
    }

    private void Update()
    {
        if (_isGlidingIntended && IsFalling && IsGliding == false)
        {
            rb.gravityScale /= gravityDivisor;
            IsGliding = true;
            OnGlideToggle.Invoke(true);
        }
        else if (IsGliding && _isGlidingIntended == false)
        {
            rb.gravityScale *= gravityDivisor;
            IsGliding = false;
            OnGlideToggle.Invoke(false);
        }
    }
}