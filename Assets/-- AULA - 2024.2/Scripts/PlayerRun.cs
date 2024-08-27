using _6.AcaoReacao;
using UnityEngine;

namespace Marlus_Aula2024
{
    public class PlayerRun : PlayerMovementBase
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration = 10;
        [SerializeField] private float deceleration = 10;
        [SerializeField] private float turnSpeed = 20;
        [SerializeField] private bool useAcceleration = true;

        private float _targetSpeed;
        private float _speedChangeRate;

        private void Start()
        {
            TargetInput.Instance.onDirectionalAxisPressed.AddListener(SetMovementDirection);
        }

        public void FixedUpdate()
        {
            if (useAcceleration)
            {
                Accelerate();
            }
            else
            {
                MoveLinearly();
            }
        }

        public void SetMovementDirection(Vector3 direction)
        {
            TryAlignScale(direction.x);
            _targetSpeed = direction.x * maxSpeed;

        }

        public void TryAlignScale(float direction)
        {
            if (direction == 0)
            {
                return;
            }
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
            transform.localScale = scale;
        }

        public void MoveLinearly()
        {
            _velocityVector = rb.velocity;
            _velocityVector.x = _targetSpeed;
            rb.velocity = _velocityVector;
        }

        public void Accelerate()
        {
            _velocityVector = rb.velocity;

            if (_velocityVector.x == 0)
            {
                if (_targetSpeed != 0)
                {
                    _speedChangeRate = acceleration;
                }
            }
            else
            {
                if (_targetSpeed == 0)
                {
                    _speedChangeRate = deceleration;
                }
                else if (Mathf.Sign(_velocityVector.x) == Mathf.Sign(_targetSpeed))
                {
                    _speedChangeRate = acceleration;
                }
                else
                {
                    _speedChangeRate = turnSpeed;
                }
            }
            
            _velocityVector.x = Mathf.MoveTowards(_velocityVector.x, _targetSpeed, _speedChangeRate * Time.fixedDeltaTime);
            rb.velocity = _velocityVector;
            // rb.AddForce(new Vector2(_velocityVector.x,0));
        }
    }

}
