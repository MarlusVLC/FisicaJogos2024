using System;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerWalk playerWalk;
    [SerializeField] private PlayerRun playerRun;
    [SerializeField] private PlayerGlide playerGlide;
    [SerializeField] private GroundDetection groundDetection;

    private void Awake()
    {
        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerRun = GetComponent<PlayerRun>();
        playerGlide = GetComponent<PlayerGlide>();
    }

    private void Start()
    {
        playerRun.OnBufferTrigger.AddListener(TryJumpingWithMomentum);
        groundDetection.AddCallbackOnGroundDetection(playerRun.StopRunning, false);
    }

    private void FixedUpdate()
    {
        ResetValues();
        ApplyRunningBuff();
        ApplyGroundlessBuffer();
        ApplyFallingBuffer();
    }

    private void ResetValues()
    {
        playerWalk.MaxSpeed.ResetMultiplier();
        playerWalk.Acceleration.ResetMultiplier();
        playerWalk.Deceleration.ResetMultiplier();
        playerWalk.TurnSpeed.ResetMultiplier();
        playerJump.JumpVelocity.ResetMultiplier();
    }

    private void TryJumpingWithMomentum()
    {
        playerJump.JumpVelocity.multiplier = playerRun.JumpVelocity;
        playerJump.TryJumping(considerJumpIntention: true);
    }

    private void ApplyRunningBuff()
    {
        if (playerRun.IsBuffering)
        {
            playerJump.JumpVelocity.multiplier = playerRun.JumpVelocity;
        }
        
        if (playerRun.IsRunning == false)
            return;
        playerWalk.MaxSpeed.multiplier = playerRun.MaxSpeed;
        playerWalk.Acceleration.multiplier = playerRun.Acceleration;
        playerWalk.Deceleration.multiplier = playerRun.Deceleration;
        playerWalk.TurnSpeed.multiplier = playerRun.TurnSpeed;
    }

    private void ApplyGroundlessBuffer()
    {
        if (groundDetection.IsOnGround())
            return;
        playerWalk.TurnSpeed.multiplier = playerJump.HorizontalTurnSpeedMult;
    }

    private void ApplyFallingBuffer()
    {
        if (playerGlide.IsGliding == false)
            return;
        playerWalk.Deceleration.multiplier = playerGlide.DecelerationMultiplier;
        playerWalk.MaxSpeed.multiplier = playerGlide.MaxSpeedMultiplier;
    }
}