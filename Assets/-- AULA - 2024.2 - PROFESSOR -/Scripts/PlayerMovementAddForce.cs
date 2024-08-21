using UnityEngine;

public class PlayerMovementAddForce : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveForce = 10;
    public float jumpForce = 10;
    float hMoveInput;
    bool jumpInput = false;

    private bool isGrounded = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        hMoveInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
        Jump();
    }
    void CheckGround()
    {
        int mask = ~LayerMask.GetMask("Player");//Projeção da esfera contra todas layers, exceto a do jogador (~)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.4f, mask);
        isGrounded = hit.collider != null;
    }
    void ApplyMovement()
    {
        rb.AddForce(new Vector2(hMoveInput * moveForce, 0));
    }
    void Jump()
    {
        if (jumpInput && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpInput = false;
    }
}
