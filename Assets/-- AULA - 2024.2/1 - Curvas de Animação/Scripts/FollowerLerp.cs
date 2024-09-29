using UnityEngine;

public class FollowerLerp : MonoBehaviour
{
    [SerializeField] private ParticleSystem.MinMaxCurve followCurve;
    [SerializeField] private Transform target;
    [SerializeField] private float proximityRadius;
    [SerializeField] private float movementDetectionRadius;
    [SerializeField] private float rotationDetectionRadius;
    [Min((0))][SerializeField] private float movementDuration;
    [SerializeField] private bool followTargetDirection;

    private Vector3 _lastDefTargetPos;
    private Vector3 _lastDefTargetDir;
    private Vector3 _lastDefProxPos;
    private Vector3 _lastDefOriginPos;
    private Vector3 _lastDefOriginToTargetDir;
    private Vector3 _lastDefOriginDir;
    private Vector3 _nextMovementPosition;
    private Vector3 _nextMovementDirection;
    [Space]
    [ReadOnly] public float _distance;
    [ReadOnly] public float t = 0f;
    
    private void Start()
    {
        _lastDefTargetPos = target.position;
        _lastDefTargetDir = target.forward;
        _lastDefOriginPos = transform.position;
        _lastDefOriginDir =  transform.forward;
        
        _lastDefOriginToTargetDir = (_lastDefTargetPos - _lastDefOriginPos).normalized;
        DefineClosestPosition();
    }

    private void DefineClosestPosition()
    {
        if (followTargetDirection)
        {
            _lastDefProxPos = _lastDefTargetPos - (_lastDefTargetDir * proximityRadius);
        }
        else
        {
            _lastDefProxPos = _lastDefOriginPos + _lastDefOriginToTargetDir * (_distance - proximityRadius);
        }
    }

    private void LateUpdate()
    {
        if (movementDuration == 0.0f)
        {
            return;
        }
        t += Time.deltaTime / movementDuration;
        
        Move3D();
        Rotate();

        _distance = Vector3.Distance(_lastDefOriginPos, _lastDefTargetPos);

        if (HasTargetPositionOrRotationChanged == false)
        {
            return;
        }
        _lastDefTargetPos = target.position;
        _lastDefTargetDir = target.forward;
        _lastDefOriginPos = transform.position;
        _lastDefOriginDir = transform.forward;
        _lastDefOriginToTargetDir = (_lastDefTargetPos - _lastDefOriginPos).normalized;
        DefineClosestPosition();
        t = 0;
    }

    private bool HasTargetPositionOrRotationChanged =>
        Vector3.Distance(_lastDefTargetPos, target.position) >= movementDetectionRadius
        || Vector3.Distance(_lastDefTargetDir, target.forward) >= rotationDetectionRadius;

    public void Move2D()
    {
        _nextMovementPosition = Vector3.LerpUnclamped(_lastDefOriginPos, _lastDefTargetPos, followCurve.Evaluate(t));
        _nextMovementPosition.z = transform.position.z;
        transform.position = _nextMovementPosition;
    }
    
    public void Move3D()
    {
        _nextMovementPosition = Vector3.LerpUnclamped(_lastDefOriginPos, _lastDefProxPos, followCurve.Evaluate(t));
        // _nextMovementPosition.z = transform.position.z;
        transform.position = _nextMovementPosition;
    }
    
    public void Rotate()
    {
        _nextMovementDirection = Vector3.SlerpUnclamped(_lastDefOriginDir, _lastDefOriginToTargetDir, followCurve.Evaluate(t));
        transform.forward = _nextMovementDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, proximityRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_lastDefProxPos, 1f);
    }
}
