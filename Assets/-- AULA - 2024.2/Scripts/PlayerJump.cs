using _6.AcaoReacao;
using UnityEngine;

public class PlayerJump : PlayerMovementBase
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float velocityThreshold;

    public bool CanJump => _detector.IsOnGround() && Mathf.Abs(rb.velocity.y) < velocityThreshold;

    private void Start()
    {
        TargetInput.Instance.onSpaceBarPressed.AddListener(Jump);
    }
    
    public void Jump()
    {
        if(CanJump == false)
            return;
        _velocity = rb.velocity;
        _velocity.y += jumpVelocity;
        rb.velocity = _velocity;
    }
}