using UnityEngine;

namespace _6.AcaoReacao
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] public bool isStatic;
        [SerializeField] public bool isKinematic;
        [SerializeField] private float mass;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private bool useGravity;

        [SerializeField] private Vector3 acceleration;

        private Collider collider;

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
        
        public Collider Collider
        {
            get => collider == null ? GetComponent<Collider>() : collider;
            set => collider = value;
        }

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
            Move();
            if (useGravity) AddForce(Weight);
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
            acceleration = forceVector / mass;
            velocity += acceleration * Time.fixedDeltaTime;
        }

        public void AddForce(float intensity, Vector3 direction)
        {
            AddForce(direction * intensity);
        }

        public void AddResistanceForce(float intensity)
        {
            AddForce(velocity.normalized * (intensity * -1));
        }
        
        public void AddInstantaneousForce(float intensity, Vector3 direction)
        {
            if (isKinematic)
            {
                return;
            }
            var forceVector = direction * intensity;
            acceleration = forceVector / mass;
            velocity += acceleration;
        }
        
        public void Move()
        {
            transform.position += velocity * Time.fixedDeltaTime;
        }

        public void HaltMovement()
        {
            Velocity = Vector3.zero;
            IsStatic = true;
        }

        [ContextMenu("Use Mass To Define Scale")]
        private void RescaleBasedOMass()
        {
            var newScale = transform.localScale;
            newScale *= Mass / 100 ;
            transform.localScale = newScale;
        }
    }
}