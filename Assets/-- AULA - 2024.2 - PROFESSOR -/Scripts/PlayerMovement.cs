using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpVelocity = 10;
    public float jumpBufferTime = 1f;
    public float minJumpTime = 1f;
   
    private float hMoveInput = 0f;
    private float jumpBuffer = 0f;
    private bool cancelJumpInput = false;
    private float cancelJumpCounter = 0f;

    public float coyoteTime = 0.5f;
    public float coyoteTimeCounter = 0;

    private bool isGrounded = false;

    [Header("Ground Check Parameters")]
    public float floorCheckDistance = 0.4f;
    public float floorCheckRadius = 0.5f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Inputs horizontais do jogador
        hMoveInput = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump"))//input de pulo
        {
            jumpBuffer = jumpBufferTime;
        }
        if(Input.GetButtonUp("Jump"))
        {
            cancelJumpInput = true;
        }
    }
    //Função que determina se o player pode pular, usando input buffer
    private bool ShouldJump()
    {
        return isGrounded && jumpBuffer > 0f;
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        jumpBuffer = 0f;
        coyoteTimeCounter = 0;
        cancelJumpCounter = minJumpTime;
    }
    private void CheckGround()
    {
        int mask = ~LayerMask.GetMask("Player");//Projeção da esfera contra todas layers, exceto a do jogador (~)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, floorCheckRadius, Vector2.down, floorCheckDistance, mask);

        if (rb.velocity.y <= 0f && hit.collider != null)
            coyoteTimeCounter = coyoteTime;

        isGrounded = false;
        if (coyoteTimeCounter > 0)
            isGrounded = true;
        else if (hit.collider != null)
            isGrounded = true;
    }
    private void Move()
    {
        rb.velocity = new Vector2(hMoveInput * moveSpeed, rb.velocity.y);
    }

    private void TryCancelJump()
    {
        //não podemos cancelar o pulo até atingir a altura mínima
        if (cancelJumpCounter < 0f)
        {
            if (cancelJumpInput)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(0f, rb.velocity.y));
            }
            cancelJumpInput = false;
        }
    }
    //Atualização de todos times relacionados às mecânicas de pulo
    private void UpdateTimers()
    {
        jumpBuffer -= Time.fixedDeltaTime;
        coyoteTimeCounter -= Time.fixedDeltaTime;
        cancelJumpCounter -= Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        Move();
        CheckGround();
        if (ShouldJump())
        {
            Jump();
        }
        TryCancelJump();
        UpdateTimers();
    }
    private void OnDrawGizmos()
    {
        //Desenho de uma esfera colorida para representar a variável isGrounded e a área de checagem.
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down* floorCheckDistance, floorCheckRadius);
    }
}
