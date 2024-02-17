using System.Collections.Generic;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    public class CollisionManager : Singleton<CollisionManager>
    {
        private List<Collider> colliders;

        public int AddCollider(Collider collider)
        {
            colliders.Add(collider);
            return colliders.Count - 1;
        }

        //TODO(Marlus) Implement, fix implementation
        public int GetNearbyColliders(int id, float detectionRadius, out Collider[] nearbyColliders)
        {
            nearbyColliders = new Collider[colliders.Count];
            for (var i = 0; i < colliders.Count; i++)
            {
                if (i == id)
                {
                    continue;
                }
                // nearbyColliders[i] =
            }

            return 0; //Temp - Doesn't work
        }
    }
}