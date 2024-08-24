using System.Collections;
using System.Collections.Generic;
using _6.AcaoReacao;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerJump : PlayerMovementBase
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private int maxJumps = 1;

    [Header("Player Assist")] 
    [SerializeField] private float maxBufferTime;
    [SerializeField] private float coyoteTime;

    private bool _hasJumped = false;
    private int _jumpCount = 0;
    private float _currentBufferTime = 0.0f;
    private float _coyoteTimeCounter = 0.0f;
    private Coroutine _coyoteRoutine;
    
    public bool IsBufferingJump => _currentBufferTime <= maxBufferTime;
    public bool IsJumping => _hasJumped && rb.velocity.y > 0;

    private void Start()
    {
        TargetInput.Instance.onSpaceBarPressed.AddListener(() => TryJumping());
        
        _detector.AddCallbackOnGroundDetection(TryResetJumpCount, true);
        _detector.AddCallbackOnGroundDetection(ResetBufferTime, true);
        _detector.AddCallbackOnGroundDetection(ResetCoyoteTime, true);
        _detector.AddCallbackOnGroundDetection(() => _hasJumped = false, true);
        
        _detector.AddCallbackOnGroundDetection(TriggerCoyoteTime, false);
    }

    public bool CanJump =>
            (_detector.IsOnGround()
            || _jumpCount < maxJumps)
            && _coyoteTimeCounter < coyoteTime;

    public bool TryJumping() => TryJumping(true);

    private bool TryJumping(bool shouldUseBuffer)
    {
        if (CanJump == false)
        {
            if (shouldUseBuffer)
            {
                StartCoroutine(TriggerBuffer());
            }

            return false;
        }
        Jump();
        return true;
    }

    public void Jump()
    {
        _jumpCount++;

        _velocity = rb.velocity;
        _velocity.y = jumpVelocity;
        rb.velocity = _velocity;
        _hasJumped = true;
        ResetCoyoteTime();
    }
    
    #region BUFFER
    private IEnumerator TriggerBuffer()
    {
        while (_currentBufferTime <= maxBufferTime)
        {
            if (TryJumping(false))
            {
                break;
            }

            yield return new WaitForEndOfFrame();
            _currentBufferTime += Time.deltaTime;
        }
    }
    
    private void ResetBufferTime() => _currentBufferTime = 0f;
#endregion

#region COYOTE_TIME
    private IEnumerator TriggerCoyoteRoutine()
    {
        while (_coyoteTimeCounter < coyoteTime)
        {
            yield return new WaitForEndOfFrame();
            _coyoteTimeCounter += Time.deltaTime;
        }
    }
        
    private void ResetCoyoteTime()
    {
        _coyoteTimeCounter = 0f;
        if (_coyoteRoutine == null)
        {
            return;
        }
        StopCoroutine(_coyoteRoutine);
        _coyoteRoutine = null;
        Debug.Log("Coyote routine cleaned!");
    }

    private void TriggerCoyoteTime()
    {
        if (IsJumping)
        {
            return;
        }
        ResetCoyoteTime();
        _coyoteRoutine = StartCoroutine(TriggerCoyoteRoutine());
    }
    #endregion

    
// Zera a contagem de pulos executados antes do jogador encostar no chão
    private void TryResetJumpCount()
    {
        if (IsJumping)
        {
            return;
        }
        _jumpCount = 0;
    }
} 