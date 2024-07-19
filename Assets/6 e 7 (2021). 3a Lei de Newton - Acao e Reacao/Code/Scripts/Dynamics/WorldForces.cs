using UnityEngine;

namespace _6.AcaoReacao
{
    public class WorldForces : Singleton<WorldForces>
    {
        [SerializeField] private Vector3 gravityAcceleration;
        [SerializeField] private float windMaximumForceMagnitude;
        [SerializeField] private Transform surfaceReferential;

        private Vector3 windForce;

        public static Vector3 EarthGravity => Instance.gravityAcceleration;
        public static Vector3 WindForce => Instance.windForce;
        public const float UniversalGravitationalConstant = 0.00000667428f;
        

        private void OnEnable()
        {
            if (TargetInput.HasInstance)
                TargetInput.Instance.onDirectionalAxisPressed.AddListener(SetWindForce);
        }

        public float GetPotentialGravitationalEnergy(RigidBody rb)
            => rb.Mass * EarthGravity.magnitude * (rb.transform.position.y - surfaceReferential.position.y);

        private void SetWindForce(Vector3 direction)
        {
            windForce = windMaximumForceMagnitude * direction;
        }
    }
}