using UnityEngine;

namespace _5.Dinamica
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private float mass;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private bool useGravity;

        private Vector3 acceleration;

        //TODO: Tentar fazer meu próprio transform.
        public float Density => mass / (transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.z);
        public Vector3 Weight => mass * WorldForces.Gravity;
        public Vector3 Momentum => mass * Velocity;

        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        private void FixedUpdate()
        {
            if (useGravity) AddForce(Weight);
            Move();
        }
        
        private void OnValidate()
        {
            mass = Mathf.Clamp(mass, 0.0000000001f, float.MaxValue);
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