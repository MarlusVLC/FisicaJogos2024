using System;
using _6.AcaoReacao;
using UnityEngine;

public class PlayerMovement_Broken : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speedModule;

    private Vector2 _velocity;
    private GroundDetection _detector;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _detector = GetComponent<GroundDetection>();
    }

    private void Start()
    {
        TargetInput.Instance.onDirectionalAxisPressed.AddListener(SetVelocity);
        Debug.Log("Directionals should make it move, now");
        TargetInput.Instance.onSpaceBarPressed.AddListener(Jump);
        Debug.Log("Space bar should make player jump, now");
    }

    private void Update()
    {
        // _velocity = rb.velocity;
        rb.velocity = _velocity;
    }

    // private void FixedUpdate()
    // {
    //     rb.velocity = _velocity;
    // }

    public void SetVelocity(Vector3 direction)
    {
        _velocity = rb.velocity;
        _velocity.x = direction.x * speedModule;
    }

    public void Jump()
    {
        Debug.Log("Should be Jumping");
        if(_detector.IsOnGround() == false)
            return;
        _velocity = rb.velocity;
        Debug.Log("IS JUMPING!");
        _velocity.y += jumpForce;
    }
}
