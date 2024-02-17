using UnityEngine;

namespace _6.AcaoReacao
{
    public class SystemManager : MonoBehaviour
    {
        [SerializeField] private RigidBody rigidBody;
        [SerializeField] private TargetInput targetInput;
        [SerializeField] private float propulsionIntensity;
        [SerializeField] private Transform floor;

        private void OnEnable()
        {
            targetInput.onMouseLeft.AddListener(AddImpulse);
            targetInput.onMouseRightDown.AddListener(AddExplosiveImpulse);
            targetInput.onDirectionalAxisPressed.AddListener(AddDirectionalImpulse);
        }

        private void OnDisable()
        {
            targetInput.onMouseLeft.RemoveAllListeners();
        }

        private void Update()
        {
            TryInvertVelocity();
        }

        private void AddDirectionalImpulse(Vector3 direction)
        {
            rigidBody.AddForce(propulsionIntensity, direction);
        }

        private void AddImpulse(Vector3 target)
        {
            var direction = (target - rigidBody.transform.position).normalized;
            rigidBody.AddForce(propulsionIntensity, direction);
        }
        
        private void AddExplosiveImpulse(Vector3 target)
        {
            var direction = (target - rigidBody.transform.position).normalized;
            rigidBody.AddInstantaneousForce(propulsionIntensity, direction);
        }

        private void TryInvertVelocity()
        {
            if (!(rigidBody.transform.position.y <= floor.position.y) || !(rigidBody.Velocity.y < 0))
            {
                return;
            }
            var restitution = rigidBody.Velocity;
            restitution.y *= -1;
            rigidBody.Velocity = restitution;
        }
    }
}