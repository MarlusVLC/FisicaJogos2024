using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private float detectionReach = 1.0f;
    [SerializeField] private float detectionAmplitude = 0.5f;

    private RaycastHit2D[] groundHits = new RaycastHit2D[2];

    public ReadOnlyArray<RaycastHit2D> GroundHits => groundHits;

    public bool IsOnGround()
    {
        int hits = Physics2D.CircleCast(
            origin: transform.position,
            radius: detectionAmplitude,
            direction: Vector2.down,
            contactFilter: contactFilter,
            results: groundHits,
            distance: detectionReach);
        // Debug.Log("Hit quantity = " + hits);
        // for (int i = 0; i < hits; i++)
        // {
        //     Debug.Log("Hit index = " + i);
        //     Debug.Log("Collider name = " + groundHits[i].transform.name);
        //     Debug.Log("Collision normal = " + groundHits[i].normal);
        // }
        return hits > 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsOnGround() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * detectionReach), detectionAmplitude);
    }
} 