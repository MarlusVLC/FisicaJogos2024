using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace _6.AcaoReacao
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] public bool isStatic;
        [SerializeField] public bool isKinematic;
        [SerializeField] private float mass;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private Vector3 acceleration;
        [SerializeField] private bool useGravity = true;
        [SerializeField] private bool useWind = true;
        [Header("Sleep Setup")]
        [SerializeField] private float accelerationSleepThreshold = 5.0f;
        [SerializeField] private int iterationUntilSleep = 4;
        [Header("Editor Only")]
        [SerializeField] private Vector3 refScale;

        private bool isAsleep = false;
        private int sleepCountdown;
        private BoundingShape boundingShape;

        public float Mass => mass;
        public float Density => mass / (transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.z);
        public Vector3 Weight => mass * WorldForces.EarthGravity;
        public Vector3 Momentum => mass * Velocity;
        public float KineticEnergy => mass * Velocity.sqrMagnitude / 2;
        public float GravitationalPotentialEnergy => WorldForces.Instance.GetPotentialGravitationalEnergy(this);

        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = IsStatic ? Vector3.zero : value;
        }
        
        public BoundingShape BoundingShape => boundingShape == null ? GetComponent<BoundingShape>() : boundingShape;

        public bool IsStatic
        {
            get => isStatic;
            set
            {
                Velocity = Vector3.zero;
                isStatic = value;
            }
        }
        
        private void FixedUpdate()
        {
            if (isStatic)
            {
                return;
            }

            // CheckSleepiness();

            if (isAsleep == false)
            {
                Move();
                if (useGravity) AddForce(Weight);
            }
            if (useWind) AddForce(WorldForces.WindForce);
        }
        
        private void OnValidate()
        {
            mass = Mathf.Clamp(mass, 0, float.MaxValue);
        }
        
        public void AddForce(Vector3 forceVector)
        {
            if (isKinematic)
            {
                return;
            }

            if (forceVector.sqrMagnitude == 0)
            {
                return;
            }
            isAsleep = false;
            acceleration += forceVector / mass;
            velocity += acceleration * Time.fixedDeltaTime;
        }

        public void AddForce(float intensity, Vector3 direction)
        {
            AddForce(direction * intensity);
        }

        public void AddOppositeForce(float intensity)
        {
            AddForce(velocity.normalized * (intensity * -1));
        }
        
        public void AddInstantaneousForce(float intensity, Vector3 direction)
        {
            if (isKinematic)
            {
                return;
            }
            isAsleep = false;
            var forceVector = direction * intensity;
            acceleration = forceVector / mass;
            velocity += acceleration;
        }
        
        public void Move()
        {
            acceleration = Vector3.zero;
            transform.position += velocity * Time.fixedDeltaTime;
        }

        public void HaltMovement()
        {
            Velocity = Vector3.zero;
            IsStatic = true;
        }

        private void CheckSleepiness()
        {
            if (acceleration.sqrMagnitude < accelerationSleepThreshold * accelerationSleepThreshold)
            {
                if (sleepCountdown < iterationUntilSleep)
                {
                    sleepCountdown++;
                }
                else
                {
                    isAsleep = true;
                    acceleration = Vector3.zero;
                    velocity = Vector3.zero;
                }
            }
            else
            {
                sleepCountdown = 0;
                isAsleep = false;
            }
        }

        [ContextMenu("Use Mass To Define Scale")]
        private void RescaleBasedOMass()
        {
            var newScale = refScale;
            newScale *= Mass / 100 ;
            transform.localScale = newScale;
        }
    }
}