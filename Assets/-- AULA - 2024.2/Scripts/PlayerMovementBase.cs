using UnityEngine;

public class PlayerMovementBase : MonoBehaviour
{
    protected GroundDetection _detector;
    protected Rigidbody2D rb;
    protected Vector2 _velocity;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _detector = GetComponent<GroundDetection>();
    }
}