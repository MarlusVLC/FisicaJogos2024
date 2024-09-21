using System;
using System.Collections;
using _6.AcaoReacao;
using UnityEngine;

public class PlayerJump : PlayerMovementBase
{
    [field: SerializeField] public MultipliableFloat JumpVelocity  { get; private set; }
    [SerializeField] private bool usePredefinedVelocity;
    [SerializeField] private bool useForce;
    [Min(0.1f)][SerializeField] private float timeToPeak;
    [Min(0.1f)][SerializeField] private float maxJumpHeight;
    [Min(0.1f)][SerializeField] private float minJumpHeight;
    [Min(0.1f)][SerializeField] private float jumpCancelRate;
    [Min(0)][SerializeField] private int maxJumps = 1;

    [Header("Player Assist")] 
    [SerializeField] private float maxBufferTime;
    [SerializeField] private float coyoteTime;
    
    [field: SerializeField] public float HorizontalTurnSpeedMult { get; private set; }

    private bool _hasJumped = false;
    private bool _isJumpIntended = false;
    private int _jumpCount = 0;
    private float _currentBufferTime = 0.0f;
    private float _coyoteTimeCounter = 0.0f;
    private float _jumpOriginY;
    private Coroutine _coyoteRoutine;
    
    public bool IsBufferingJump => _currentBufferTime <= maxBufferTime;
    public bool IsJumping => _hasJumped && rb.velocity.y >= 0;
    public bool IsFalling => _detector.IsOnGround() == false && rb.velocity.y < 0;
    public bool CanUseBuffer => maxBufferTime > 0.0f;
    public bool CanUseCoyoteTime => coyoteTime > 0.0f;

    private void Start()
    {
        TargetInput.Instance.onSpaceBarPressed.AddListener(() => TryJumping(shouldUseBuffer: CanUseBuffer));
        TargetInput.Instance.onSpaceBarReleased.AddListener(TriggerJumpCancelRoutine);
        
        _detector.AddCallbackOnGroundDetection(TryResetJumpCount, true);
        _detector.AddCallbackOnGroundDetection(ResetBufferTime, true);
        _detector.AddCallbackOnGroundDetection(ResetCoyoteTime, true);
        _detector.AddCallbackOnGroundDetection(() => _hasJumped = false, true);
        
        _detector.AddCallbackOnGroundDetection(TriggerCoyoteTime, false);
    }

    public bool CanJump =>
        (_detector.IsOnGround()
         || _jumpCount < maxJumps)
        && (_coyoteTimeCounter < coyoteTime || CanUseCoyoteTime == false)
        && _playerRun.IsRunning == false
    ;
    
    public bool TryJumping(bool shouldUseBuffer = false, bool considerJumpIntention = false)
    {
        if (considerJumpIntention)
        {
            if (_isJumpIntended == false) 
                return false;
            Jump();
            return true;
        }
        
        if (CanJump == false)
        {
            _isJumpIntended = true;
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
        DefineJumpVelocity();

        if (useForce)
        {
            if (_jumpCount > 1)
            {
                SetVelocity(y: 0);
            }

            rb.AddForce(Vector2.up * (rb.mass * JumpVelocity), ForceMode2D.Impulse);
        }
        else
        {
            SetVelocity(y: JumpVelocity);
        }
        _jumpOriginY = transform.position.y;
        _hasJumped = true;
        ResetCoyoteTime();
    }

    public void DefineJumpVelocity()
    {
        if (usePredefinedVelocity)
        {
            return;
        }
        float gravity = 2 * maxJumpHeight / Mathf.Pow(timeToPeak, 2);
        rb.gravityScale = gravity / Mathf.Abs(Physics2D.gravity.y);
        JumpVelocity.baseValue = 2 * (maxJumpHeight / timeToPeak);
    }
    
    public float GetCurrentJumpHeight()
    {
        if (IsJumping == false)
        {
            throw new NullReferenceException("Player hasn't jumped yet. Therefore no Jump Height can be calculated");
        }
        return transform.position.y - _jumpOriginY;
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

    private IEnumerator CancelJump()
    {
        _isJumpIntended = false;
        while (IsJumping && GetCurrentJumpHeight() < minJumpHeight )
        {
            yield return new WaitForFixedUpdate();
        }
        var targetSpeed = Mathf.MoveTowards(rb.velocity.y, 0, jumpCancelRate);
        SetVelocity(y: Mathf.Min(targetSpeed, rb.velocity.y));
    }

    public void TriggerJumpCancelRoutine()
    {
        StartCoroutine(CancelJump());
    }

    
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