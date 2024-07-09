using UnityEngine;

namespace _6.AcaoReacao
{
    public class WorldForces : Singleton<WorldForces>
    {
        public static Vector3 EarthGravity = Vector3.down * 9.8f;

        public Transform surfaceReferential;

        public float GetPotentialGravitationalEnergy(RigidBody rb)
            => rb.Mass * EarthGravity.magnitude * (rb.transform.position.y - surfaceReferential.position.y);
    }
}