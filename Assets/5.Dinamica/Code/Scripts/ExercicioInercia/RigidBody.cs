using UnityEngine;

namespace _5.Dinamica
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private float mass;
        [SerializeField] private Vector3 velocity;

        private Vector3 acceleration;

        public float Density => mass / (transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.y);
        public Vector3 Weight => mass * WorldForces.Gravity;

        private void FixedUpdate()
        {
            Move();
        }
        
        private void OnValidate()
        {
            mass = Mathf.Clamp(mass, 0.0000000001f, float.MaxValue);
        }

        public void AddForce(float intensity, Vector3 direction)
        {
            var force = direction * intensity;
            velocity += force / mass * Time.fixedDeltaTime;
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