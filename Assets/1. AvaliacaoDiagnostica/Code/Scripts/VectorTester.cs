using System;
using UnityEngine;

namespace _1._AvaliacaoDiagnostica.Code.Scripts
{
    public class VectorTester : MonoBehaviour
    {
        [SerializeField] private CustomVector a;
        [SerializeField] private CustomVector b;

        [ContextMenu("Sum Into A")]
        public void SumIntoA() => a += b;
        
        [ContextMenu("Decrease A")]
        public void DecreaseA() => a -= b;

        [ContextMenu("Normalize A")] 
        public void NormalizeA() => a = a.Normalize();

        [ContextMenu("Duplicate A")]
        public void DuplicateA() => a *= 2;
        
        [ContextMenu("Half A")]
        public void HalfA() => a /= 2;
        
        [ContextMenu("Show A's Magnitude")]
        public void ShowAMagnitude() => Debug.Log(a.Magnitude());

    }
}