using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(RigidBody))]
    public class SlingshotController : MonoBehaviour
    {
        [SerializeField] private RigidBody projectile;
        [SerializeField] private TargetInput targetInput;
        [SerializeField] private Transform slingshotForkCenter;
        [SerializeField] private float elasticConstant;
        [SerializeField] private bool preserveMomentum;
        
        private RigidBody slingshotRb;
        private Vector3 originalLocalProjPosition;
        private Quaternion originalLocalProjRotation;

        private void Awake()
        {
            slingshotRb = GetComponent<RigidBody>();
        }

        private void OnEnable()
        {
            targetInput.onMouseLeft.AddListener(Stretch);
            targetInput.onMouseLeftUp.AddListener(Fire);
            targetInput.onSpaceBarPressed.AddListener(ResetProjectile);
        }

        private void Start()
        {
            projectile.transform.parent = slingshotRb.transform;
            projectile.IsStatic = true;
            projectile.transform.GetLocalPositionAndRotation(out originalLocalProjPosition, out originalLocalProjRotation);
            
            // projectile.Collider.onCollision.AddListener(HaltMovement);
        }

        private void AddExplosiveImpulse()
        {
            var direction = (slingshotForkCenter.position - projectile.transform.position).normalized;
            var propulsionIntensity =
                elasticConstant * Vector3.Distance(slingshotForkCenter.position, projectile.transform.position);
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
            AddExplosiveImpulse();

            if (preserveMomentum)
            {
                slingshotRb.Velocity = -(projectile.Momentum / slingshotRb.Mass);
            }
        }

        private void Stretch(Vector3 target)
        {
            if (projectile.transform.parent != transform)
            {
                return;
            }
            projectile.transform.position = target;
        }

        private void ResetProjectile() 
        {
            projectile.IsStatic = projectile.isKinematic = true;
            projectile.transform.SetParent(transform);
            projectile.transform.SetLocalPositionAndRotation(originalLocalProjPosition, originalLocalProjRotation);
            projectile.Collider.enabled = true;
        }

        private void HaltMovement(Collider stopper)
        {
            projectile.Collider.enabled = false;
            projectile.IsStatic = true;
        }
    }
}