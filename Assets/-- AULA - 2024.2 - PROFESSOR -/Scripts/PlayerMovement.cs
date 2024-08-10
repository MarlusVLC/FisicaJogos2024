using UnityEngine;

namespace Professor
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5;
        public float jumpVelocity = 10;
        
        private Rigidbody2D rb;
        private GroundDetection groundDetection;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            groundDetection = GetComponent<GroundDetection>();
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
                if (groundDetection.IsOnGround())
                {
                    newVelocity.y = jumpVelocity;
                }
            }

            rb.velocity = newVelocity;
        }
    }
}

