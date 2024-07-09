using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(RigidBody))]
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private RigidBody projectile;
        [SerializeField] private TargetInput targetInput;
        [SerializeField] private float propulsionIntensity;
        [SerializeField] private bool preserveMomentum;
        
        private RigidBody cannonRb;
        private Vector3 originalLocalProjPosition;
        private Quaternion originalLocalProjRotation;

        private void Awake()
        {
            cannonRb = GetComponent<RigidBody>();
        }

        private void OnEnable()
        {
            targetInput.onMouseLeftDown.AddListener(Fire);
            targetInput.onSpaceBarPressed.AddListener(ResetProjectile);
        }

        private void Start()
        {
            projectile.transform.parent = cannonRb.transform;
            projectile.IsStatic = true;
            projectile.transform.GetLocalPositionAndRotation(out originalLocalProjPosition, out originalLocalProjRotation);
        }

        private void Update()
        {
            RotateTowards(targetInput.GetMousePosition());
        }
        
        public void RotateTowards(Vector3 target)
        {
            var direction = (target - transform.position).normalized;
            var crossUptoDirection = transform.right.x * direction.y - direction.x * transform.right.y;
            if (crossUptoDirection == 0) crossUptoDirection = 1;
            var rotationDirection = crossUptoDirection / Mathf.Abs(crossUptoDirection);
            var currentToTarget = VectorN.Angle(transform.right, direction) * rotationDirection;
            if (float.IsNaN(currentToTarget))
            {
                return;
            }
            transform.eulerAngles += Vector3.forward * (currentToTarget * Mathf.Rad2Deg);
        }
        
        private void AddExplosiveImpulse(Vector3 target)
        {
            var direction = (target - transform.position).normalized;
            projectile.AddInstantaneousForce(propulsionIntensity, direction);
        }

        private void Fire(Vector3 target)
        {
            if (projectile.transform.parent != transform)
            {
                return;
            }
            projectile.IsStatic = projectile.isKinematic = false;
            projectile.transform.SetParent(null, true);
            AddExplosiveImpulse(target);

            if (preserveMomentum)
            {
                cannonRb.Velocity = -(projectile.Momentum / cannonRb.Mass);
            }
        }

        private void ResetProjectile() 
        {
            projectile.IsStatic = projectile.isKinematic = true;
            projectile.transform.SetParent(transform);
            projectile.transform.SetLocalPositionAndRotation(originalLocalProjPosition, originalLocalProjRotation);
        }
    }
}