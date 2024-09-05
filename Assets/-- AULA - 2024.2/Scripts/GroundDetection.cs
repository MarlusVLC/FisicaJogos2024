using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private ContactFilter2D contactFilter;
    [FormerlySerializedAs("detectionReach")] [SerializeField] private float groundCheckDistance = 1.0f;
    [FormerlySerializedAs("detectionAmplitude")] [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private UnityEvent onGroundDetected;
    [SerializeField] private UnityEvent onGroundLost;

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
        int hits = Physics2D.CircleCast(
            origin: transform.position,
            radius: GroundCheckRadius,
            direction: Vector2.down,
            contactFilter: contactFilter,
            results: _groundHits,
            distance: GroundCheckDistance);
        // Debug.Log("Hit quantity = " + hits);
        // for (int i = 0; i < hits; i++)
        // {
        //     Debug.Log("Hit index = " + i);
        //     Debug.Log("Collider name = " + groundHits[i].transform.name);
        //     Debug.Log("Collision normal = " + groundHits[i].normal);
        // }
        return hits > 1;
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
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * GroundCheckDistance), GroundCheckRadius);
    }
} 