using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _3.Cinematica
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] private float translationDuration;
        [SerializeField] private float rotationDuration;

        private Coroutine translationMovement;
        private Coroutine rotationMovement;
        
        public UnityEvent<Vector3> onTranslationCompleted;
        public UnityEvent<Vector3> onRotationCompleted;

        public IEnumerator MoveUniformly(Vector3 target, float movementDuration)
        {
            var currentToTarget = (target - transform.position);
            var velocity = currentToTarget  * Time.fixedDeltaTime / movementDuration;

            var timer = 0.0f;
            while (timer < movementDuration) 
            {
                transform.position += velocity;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            onTranslationCompleted.Invoke(target);
        }

        public void BeginNewTranslation(Vector3 target)
        {
            if (translationMovement == null)
            {
                translationMovement = StartCoroutine(MoveUniformly(target, translationDuration));
            }
            else
            {
                StopCoroutine(translationMovement);
                translationMovement = StartCoroutine(MoveUniformly(target, translationDuration));
            }
        }
        
        public IEnumerator RotateUniformly(Vector3 target, float movementDuration)
        {
            var direction = (target - transform.position).normalized;
            var angle = VectorN.Angle(transform.up, direction, true);
            var angularDistance = angle - transform.rotation.eulerAngles.z;
            Debug.Log(angularDistance);
            // var velocity = currentToTarget  * Time.fixedDeltaTime / movementDuration;

            var timer = 0.0f;
            while (timer < movementDuration) 
            {
                // transform.position += velocity;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            onRotationCompleted.Invoke(target);
        }
        
        public void BeginNewRotation(Vector3 target)
        {
            if (rotationMovement == null)
            {
                rotationMovement = StartCoroutine(RotateUniformly(target, rotationDuration));
            }
            else
            {
                StopCoroutine(rotationMovement);
                rotationMovement = StartCoroutine(RotateUniformly(target, rotationDuration));
            }
        }
    }
}