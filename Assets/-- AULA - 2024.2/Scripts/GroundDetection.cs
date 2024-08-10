using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectionReach = 1.0f;
    [SerializeField] private float detectionAmplitude = 0.5f;
    
    public bool IsOnGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position, 
            detectionAmplitude, 
            Vector2.down, 
            detectionReach,
            groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsOnGround() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * detectionReach), detectionAmplitude);
    }
}