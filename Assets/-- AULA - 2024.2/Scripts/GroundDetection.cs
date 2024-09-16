using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;

public enum CastType
{
    Circle, Box, Capsule
}

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private CastType castType;
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private float groundCheckDistance = 1.0f;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private UnityEvent onGroundDetected;
    [SerializeField] private UnityEvent onGroundLost;
    [Header("Debug Settings")]
    [SerializeField] 

    private bool _wasOnGround; //Was on ground in the last frame
    private RaycastHit2D[] _groundHits = new RaycastHit2D[2];

    public ReadOnlyArray<RaycastHit2D> GroundHits => _groundHits;
    public float GroundCheckDistance => groundCheckDistance * transform.localScale.y;
    public float GroundCheckRadius => groundCheckRadius * Math.Abs(transform.localScale.x);

    public void Update()
    {
        CheckDetectionStatus();
    }

    private void CheckDetectionStatus()
    {
        var isOnGround = IsOnGround();
        if (_wasOnGround == isOnGround) 
            return;
        
        _wasOnGround = isOnGround;
        if (isOnGround)
        {
            onGroundDetected.Invoke();
        }
        else
        {
            onGroundLost.Invoke();
        }
    }

    public bool IsOnGround()
    {
        int hits = 0;

        switch (castType)
        {
            case CastType.Circle:
                hits = Physics2D.CircleCast(
                    origin: transform.position,
                    radius: GroundCheckRadius,
                    direction: Vector2.down,
                    contactFilter: contactFilter,
                    results: _groundHits,
                    distance: GroundCheckDistance);
                break;

            case CastType.Box:
                hits = Physics2D.BoxCast(
                    origin: transform.position,
                    size: new Vector2(GroundCheckRadius * 2, GroundCheckRadius),
                    angle: 0f, // No rotation (can be parameterized if needed)
                    direction: Vector2.down,
                    contactFilter: contactFilter,
                    results: _groundHits,
                    distance: GroundCheckDistance);
                break;

            case CastType.Capsule:
                hits = Physics2D.CapsuleCast(
                    origin: transform.position,
                    size: new Vector2(GroundCheckRadius * 2, GroundCheckRadius * 2),
                    capsuleDirection: CapsuleDirection2D.Vertical, // Vertical capsule for ground detection
                    angle: 0f, // No rotation (can be parameterized)
                    direction: Vector2.down,
                    contactFilter: contactFilter,
                    results: _groundHits,
                    distance: GroundCheckDistance);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        
        return hits > 1;
    }

    private void DebugHits(RaycastHit2D[] hits)
    {
        Debug.Log("Hit quantity = " + hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Hit index = " + i);
            Debug.Log("Collider name = " + hits[i].transform.name);
            Debug.Log("Collision normal = " + hits[i].normal);
        }
    }


    public void AddCallbackOnGroundDetection(UnityAction action, bool doItWhenGrounding)
    {
        if (doItWhenGrounding)
            onGroundDetected.AddListener(action);
        else
            onGroundLost.AddListener(action);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsOnGround() ? Color.green : Color.red;

        switch (castType)
        {
            case CastType.Circle:
                Gizmos.DrawWireSphere(transform.position + (Vector3.down * GroundCheckDistance), GroundCheckRadius);
                break;

            case CastType.Box:
                Gizmos.DrawWireCube(transform.position + (Vector3.down * GroundCheckDistance),
                    new Vector3(GroundCheckRadius * 2, GroundCheckRadius, 1)); // Box size
                break;

            case CastType.Capsule:
                // Gizmos doesn't have a built-in capsule, so we'll approximate one
                Gizmos.DrawWireSphere(transform.position + (Vector3.down * GroundCheckDistance) + Vector3.up * GroundCheckRadius, GroundCheckRadius); // Top hemisphere
                Gizmos.DrawWireSphere(transform.position + (Vector3.down * GroundCheckDistance) - Vector3.up * GroundCheckRadius, GroundCheckRadius); // Bottom hemisphere
                Gizmos.DrawWireCube(transform.position + (Vector3.down * GroundCheckDistance),
                    new Vector3(GroundCheckRadius * 2, GroundCheckRadius * 2)); // Capsule body
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
} 