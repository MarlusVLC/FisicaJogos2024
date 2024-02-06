using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace _3.Cinematica
{
    public class BasicTargetedMovement : MonoBehaviour
    {
        public Action onMovementCompleted;

        public IEnumerator MoveUniformly(Vector3 target, float movementDuration)
        {
            var currentToTarget = (VectorN)(target - transform.position);
            var velocity = currentToTarget / movementDuration;
            float[,] tmat =
            {
                { 1, 0, velocity[0] }, 
                { 0, 1, velocity[1] },
                { 0, 0, 1}
            };

            var timer = 0.0f;
            while (timer < 4.0f)
            {
                transform.position = VectorN.MultiplyByHomogeneousMatrix(transform.position, tmat);
                timer += Time.fixedTime;
                yield return new WaitForFixedUpdate();
            }
            onMovementCompleted.Invoke();
        }
    }
}