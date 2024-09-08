using System.Collections;
using _6.AcaoReacao;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRun : PlayerMovementBase
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

    public bool IsRunning => _isRunning && _detector.IsOnGround() && Mathf.Abs(rb.velocity.x) > 0;
    public bool IsBuffering => _currentBufferTime < maxBufferTime;

    protected override void Awake()
    {
        base.Awake();
        _detector = GetComponent<GroundDetection>();
        rb = GetComponent<Rigidbody2D>();
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