﻿using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public class ProtoBoxCollider : Collider
    {
        [field: SerializeField] public Vector3 CollisionSize { get; private set; } = Vector3.one;
        [SerializeField] private bool showDetailedGizmos;

        public override Vector3 Size =>
            new(
                CollisionSize.x * transform.lossyScale.x,
                CollisionSize.y * transform.lossyScale.y,
                CollisionSize.z * transform.lossyScale.z
            );
        


        //
        // protected override void NarrowPhaseCheck()
        // {
        //     for (var i = 0; i < NearbyColliders.Length; i++)
        //     {
        //         if (NearbyColliders[i] == null)
        //         {
        //             break;
        //         }
        //         onCollision.Invoke(NearbyColliders[i]);
        //     }
        // }
        
        protected override void DrawShape()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Center, Size);
            if (showDetailedGizmos == false)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PosVertex, 0.5f);
            Gizmos.DrawWireSphere(NegVertex, 0.5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(PosVertex.x, Center.y, Center.z), 0.25f);
            Gizmos.DrawWireSphere( new Vector3(Center.x, PosVertex.y, Center.z), 0.25f);
            Gizmos.DrawWireSphere(new Vector3(Center.x, Center.y, PosVertex.z), 0.25f);
            Gizmos.DrawWireSphere(new Vector3(NegVertex.x, Center.y, transform.position.z), 0.25f);
            Gizmos.DrawWireSphere( new Vector3(Center.x, NegVertex.y, Center.z), 0.25f);
            Gizmos.DrawWireSphere(new Vector3(Center.x, Center.y, NegVertex.z), 0.25f);
        }
    }
}