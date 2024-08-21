using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpVelocity = 10;
    public float jumpBufferTime = 1f;

    private float hMoveInput = 0f;
    private float jumpBuffer = 0f;
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
    }
    //Função que determina se o player pode pular, usando input buffer
    private bool ShouldJump()
    {
        return IsGrounded() && jumpBuffer > 0f;
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        jumpBuffer = 0f;
    }
    private bool IsGrounded()
    {
        int mask = ~LayerMask.GetMask("Player");//Projeção da esfera contra todas layers, exceto a do jogador (~)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.4f, mask);
        return hit.collider != null;
    }
    private void Move()
    {
        rb.velocity = new Vector2(hMoveInput * moveSpeed, rb.velocity.y);
    }
    private void FixedUpdate()
    {
        Move();
        if (ShouldJump())
        {
            Jump();
        }
        jumpBuffer -= Time.fixedDeltaTime;
    }
    private void OnDrawGizmos()
    {
        //Desenho de uma esfera colorida para representar a variável isGrounded e a área de checagem.
        Gizmos.color = IsGrounded() ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down* 0.4f, 0.5f);
    }
}
