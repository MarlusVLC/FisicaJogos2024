using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpVelocity = 10;
    public float jumpBufferTime = 1f;
    public float coyoteTime = 1f;
    public float variableJumpMinTime = 1f;

    private float hMoveInput = 0f;
    private float jumpBuffer = 0f;
    private float coyoteTimer = 0f;
    private float variableJumpTimer = 0f;
    private bool cancelJumpInput = false;

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
        return IsGrounded() && jumpBuffer > 0f;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        jumpBuffer = 0f;
        coyoteTimer = 0f;
        variableJumpTimer = variableJumpMinTime;
    }

    private bool IsGrounded()
    {
        int mask = ~LayerMask.GetMask("Player");//Projeção da esfera contra todas layers, exceto a do jogador (~)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.4f, mask);
        if (hit.collider != null && rb != null && rb.velocity.y <= 0f)
        {
            coyoteTimer = coyoteTime;
        }
        return coyoteTimer > 0f || hit.collider != null;
    }

    private void Move()
    {
        rb.velocity = new Vector2(hMoveInput * moveSpeed, rb.velocity.y);
    }

    private void TryCancelJump()
    {
        //Checagem do timer, pois o pulo só pode ser cancelado após o tempo mínimo
        if (variableJumpTimer < 0f)
        {
            if (cancelJumpInput)
            {
                //Cancelamento da velocidade vertical. Se a velocidade for menor que 0, não fazemos nada
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, 0f));
            }
            cancelJumpInput = false;
        }
    }

    //Atualização dos timers relacionados a técnicas de movimentação
    private void UpdateTimers()
    {
        jumpBuffer -= Time.fixedDeltaTime;
        coyoteTimer -= Time.fixedDeltaTime;
        variableJumpTimer -= Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        Move();
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
        Gizmos.color = IsGrounded() ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down* 0.4f, 0.5f);
    }
}
