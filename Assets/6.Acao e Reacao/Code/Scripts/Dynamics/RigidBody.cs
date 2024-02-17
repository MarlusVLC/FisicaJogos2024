using UnityEngine;

namespace _6.AcaoReacao
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private bool isStatic;
        [SerializeField] public bool isKinematic;
        [SerializeField] private float mass;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private bool useGravity;

        private Vector3 acceleration;

        public float Mass => mass;
        public float Density => mass / (transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.y);
        public Vector3 Weight => mass * WorldForces.Gravity;
        public Vector3 Momentum => mass * Velocity;

        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
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
            if (isKinematic)
            {
                return;
            }
            if (useGravity) AddForce(Weight);
        }
        
        private void OnValidate()
        {
            mass = Mathf.Clamp(mass, 0, float.MaxValue);
        }
        
        public void AddForce(Vector3 forceVector)
        {
            acceleration = forceVector / mass;
            velocity += acceleration * Time.fixedDeltaTime;
        }

        public void AddForce(float intensity, Vector3 direction)
        {
            AddForce(direction * intensity);
        }
        
        public void AddInstantaneousForce(float intensity, Vector3 direction)
        {
            var force = direction * intensity;
            velocity += force / mass;
        }
        
        public void Move()
        {
            transform.position += velocity * Time.fixedDeltaTime;
        }
    }
}