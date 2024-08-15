using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpVelocity = 10;

    private bool isGrounded = false;
    private float moveInput = 0f;
    private bool jumpInput = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Na função Update, fazemos a leitura de inputs apenas
    void Update()
    {
        //Inputs horizontais do jogador
        moveInput = Input.GetAxis("Horizontal");

        //Input de pulo
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }
    }

    //Na função FixedUpdate, aplicamos as operações físicas baseadas no input recebido
    //Para este exemplo de movimentação, não é TÃO errado o uso do Update, pois estamos
    //alterando a velocity, processada internamente pela Unity. Mesmo assim, aconselha-se
    //fazer modificações físicas dentro da atualização fixa.
    private void FixedUpdate()
    {
        //Obtendo a velocidade anterior
        Vector2 newVelocity = rb.velocity;
       
        //atribuindo a velocidade horizontal baseada no input
        newVelocity.x = moveInput * moveSpeed; // TODO: Isso deveria ficar no Update

        int mask = ~LayerMask.GetMask("Player");
        //TODO: A decisão sobre qual formato utilizar pode ser tomada a partir de um enum
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, mask);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,0.5f,Vector2.down,0.3f,mask);
        //RaycastHit2D hit = Physics2D.BoxCast(transform.position,new Vector2(1,1), 0, Vector2.down,0.5f, mask);

        if (hit.collider != null)
            isGrounded = true;

        isGrounded = hit.collider != null;

        if (isGrounded && jumpInput)
        {
            newVelocity.y = jumpVelocity;
        }

        rb.velocity = newVelocity;
        jumpInput = false;//Reset do input de pulo após seu uso
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.red;

        //Gizmos.DrawRay(transform.position, Vector2.down * 0.6f);
        Gizmos.DrawWireSphere(transform.position + Vector3.down*0.3f, 0.5f);
        //Gizmos.DrawWireCube(transform.position + Vector3.down * 0.5f,new Vector3(1,1,1));

    }
}
