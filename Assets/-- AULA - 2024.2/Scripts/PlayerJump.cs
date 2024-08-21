using System.Collections;
using System.Collections.Generic;
using _6.AcaoReacao;
using UnityEngine;

public class PlayerJump : PlayerMovementBase
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private int maxJumps = 1; //Not used yet
    [SerializeField] private float velocityThreshold;

    [Header("Player Assist")] 
    [SerializeField] private float maxBufferTime;

    private int _jumpCount = 0;
    private float _currentBufferTime = 0.0f;

    public bool CanJump => _detector.IsOnGround()
                           || _jumpCount < maxJumps;
    // && Mathf.Abs(rb.velocity.y) < velocityThreshold
    public bool IsBufferingJump => _currentBufferTime <= maxBufferTime;
                           
    private void Start()
    {
        TargetInput.Instance.onSpaceBarPressed.AddListener(() => TryJumping());
        _detector.AddCallbackOnGroundDetection(ResetJumpCount);
        _detector.AddCallbackOnGroundDetection(ResetBufferTime);
    }

    public bool TryJumping() => TryJumping(true);
    
    private bool TryJumping(bool shouldUseBuffer)
    {
        Debug.Log("Jump Count = " + _jumpCount);
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
    }

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

    // Zera a contagem de pulos executados antes do jogador encostar no chão
    private void ResetJumpCount() => _jumpCount = 0;
    private void ResetBufferTime() => _currentBufferTime = 0f;
} 