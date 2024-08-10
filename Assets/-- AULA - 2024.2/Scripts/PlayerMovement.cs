using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpVelocity = 10;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Obtendo a velocidade anterior
        Vector2 newVelocity = rb.velocity;

        //Inputs horizontais do jogador
        float moveInput = Input.GetAxis("Horizontal");
        newVelocity.x = moveInput * moveSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            newVelocity.y = jumpVelocity;
        }

        rb.velocity = newVelocity;
    }
}