using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(RigidBody))]
    public class UniversalBody : MonoBehaviour
    {

        [SerializeField] private bool useGravitationalConstant = true;
        [Min(0)] [SerializeField] private float minDistance;
        [Min(0)] [SerializeField] private float maxDistance;
        [SerializeField] private MassRelation massRelation = MassRelation.Attraction;
        [SerializeField] private DistanceRelation distanceRelation = DistanceRelation.Closer;

        private RigidBody rb;
        private UniversalBody[] otherUniversalBodies;

        private void Awake()
        {
            rb = GetComponent<RigidBody>();
        }

        private void Start()
        {
            otherUniversalBodies = GatherOtherUniversalBodies().ToArray();
        }

        private void FixedUpdate()
        {
            ApplyRelationalForce();
        }

        private Vector3 GetGravitationalForce(UniversalBody otherBody)
        {
            Vector3 directionalVector = massRelation switch
            {
                MassRelation.Attraction => transform.position - otherBody.transform.position,
                MassRelation.Repulsion => otherBody.transform.position - transform.position,
                _ => Vector3.zero
            };
            Vector3 unitDir = directionalVector.normalized;
            var gravConstant = useGravitationalConstant ? WorldForces.UniversalGravitationalConstant : 1;
            var distance = Mathf.Clamp(Vector3.SqrMagnitude(directionalVector), minDistance * minDistance,
                maxDistance * maxDistance);
            var magnitude = distanceRelation switch
            {
                DistanceRelation.Closer => (gravConstant * otherBody.rb.Mass * rb.Mass) / distance,
                DistanceRelation.Farther => 1 / ((gravConstant * otherBody.rb.Mass * rb.Mass) / distance)
            };
            return unitDir * magnitude;
        }

        private void ApplyRelationalForce()
        {
            foreach (var body in otherUniversalBodies)
            {
                if (body.gameObject.activeInHierarchy == false || body.enabled == false)
                {
                    continue;
                }
                body.rb.AddForce(GetGravitationalForce(body));
            }
        }

        public static IEnumerable<UniversalBody> GatherAllUniversalBodies()
        {
            return FindObjectsOfType(typeof(UniversalBody), true)
                .Cast<UniversalBody>();
        }

        private IEnumerable<UniversalBody> GatherOtherUniversalBodies()
        {
            return GatherAllUniversalBodies().Where(b => b != this);
        }

        [ContextMenu("Show Distances")]
        private void ShowDistances()
        {
            foreach (var otherBody in otherUniversalBodies)
            {
                var distance = Vector3.Distance(transform.position, otherBody.transform.position);
                Debug.Log($"The distance between {name} and {otherBody.transform.position} is {distance}");
            }
        }
    }
    
    public enum MassRelation{ Attraction = 0, Repulsion = 1 }
    public enum DistanceRelation { Closer = 0, Farther = 1 }
}
