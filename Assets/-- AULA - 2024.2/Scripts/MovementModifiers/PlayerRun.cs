using System.Collections;
using _6.AcaoReacao;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRun : MonoBehaviour
{
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float Deceleration { get; private set; }
    [field: SerializeField] public float TurnSpeed { get; private set; }
    [field: SerializeField] public float JumpVelocity { get; private set; }
    [SerializeField] private float maxBufferTime;
    [field: SerializeField] public UnityEvent OnBufferTrigger { get; private set; }
    
    private bool _isRunning;
    private float? _currentBufferTime = null;
    private GroundDetection _detector;
    private Rigidbody2D _rigidBody;
    
    public bool IsRunning => _isRunning && _detector.IsOnGround() && Mathf.Abs(_rigidBody.velocity.x) > 0;
    public bool IsBuffering => _currentBufferTime < maxBufferTime;

    private void Awake()
    {
        _detector = GetComponent<GroundDetection>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        TargetInput.Instance.onShiftPressed.AddListener(() => _isRunning = true);
        TargetInput.Instance.onShiftReleased.AddListener(StopRunning);
    }

    private IEnumerator BufferRoutine()
    {
        _currentBufferTime = 0.0f;
        OnBufferTrigger.Invoke();
        while (_currentBufferTime <= maxBufferTime)
        {
            yield return new WaitForEndOfFrame();
            _currentBufferTime += Time.deltaTime;
        }
        _currentBufferTime = null; 
    }

    private void TriggerBufferRoutine()
    {
        
        StartCoroutine(BufferRoutine());
    }

    public void StopRunning()
    {
        if (!_isRunning) 
            return;
        TriggerBufferRoutine();
        _isRunning = false;
    }
}