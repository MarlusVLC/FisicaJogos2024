using UnityEngine;
using UnityEngine.Serialization;

namespace _5.Dinamica
{
    public class SystemManager : MonoBehaviour
    {
        [SerializeField] private RigidBody rigidBody;
        [SerializeField] private TargetInput targetInput;
        [SerializeField] private float propulsionIntensity;

        private void OnEnable()
        {
            targetInput.onMouseLeft.AddListener(AddImpulse);
            targetInput.onMouseRightDown.AddListener(AddExplosiveImpulse);
        }

        private void OnDisable()
        {
            targetInput.onMouseLeft.RemoveAllListeners();
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
    }
}