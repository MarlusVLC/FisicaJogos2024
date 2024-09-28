using UnityEngine;

public class FollowerLerp : MonoBehaviour
{
    [SerializeField] private ParticleSystem.MinMaxCurve followCurve;
    [SerializeField] private Transform target;
    [SerializeField] private float proximityRadius;
    [SerializeField] private float movementDetectionRadius;
    [Min((0))][SerializeField] private float movementDuration;

    private Vector3 _lastDefTargetPos;
    private Vector3 _lastDefProxPos;
    private Vector3 _lastDefOriginPos;
    private Vector3 _lastDefOriginToTargetDir;
    private Vector3 _lastDefOriginDir;
    private Vector3 _nextMovementPosition;
    private Vector3 _nextMovementDirection;
    [Space]
    public float _distance;
    public float t = 0f;
    
    private void Start()
    {
        _lastDefTargetPos = target.position;
        _lastDefOriginPos = transform.position;
        _lastDefOriginDir =  transform.forward;
        
        _lastDefOriginToTargetDir = (_lastDefTargetPos - _lastDefOriginPos).normalized;
        _lastDefProxPos = _lastDefOriginToTargetDir * (_distance - proximityRadius);
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

        if (Vector3.Distance(_lastDefTargetPos, target.position) < movementDetectionRadius )
        {
            return;
        }
        _lastDefTargetPos = target.position;
        _lastDefOriginPos = transform.position;
        _lastDefOriginDir = transform.forward;
        _lastDefOriginToTargetDir = (_lastDefTargetPos - _lastDefOriginPos).normalized;
        _lastDefProxPos = _lastDefOriginPos + _lastDefOriginToTargetDir * (_distance - proximityRadius);
        t = 0;
    }

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
