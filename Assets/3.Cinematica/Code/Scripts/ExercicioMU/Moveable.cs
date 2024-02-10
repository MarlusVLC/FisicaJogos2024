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

        public IEnumerator MoveTowards(Vector3 target, float movementDuration)
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
                translationMovement = StartCoroutine(MoveTowards(target, translationDuration));
            }
            else
            {
                StopTranslation();
                translationMovement = StartCoroutine(MoveTowards(target, translationDuration));
            }
        }

        public void StopTranslation()
        {
            if (translationMovement != null)
            {
                StopCoroutine(translationMovement);
            }
        } 
        
        public IEnumerator RotateUniformly(Vector3 target, float movementDuration)
        {
            var direction = (target - transform.position).normalized;
            var crossUptoDirection = transform.up.x * direction.y - direction.x * transform.up.y;
            if (crossUptoDirection == 0) crossUptoDirection = 1;
            var rotationDirection = crossUptoDirection / Mathf.Abs(crossUptoDirection);
            var currentToTarget = VectorN.Angle(transform.up, direction) * rotationDirection;
            var velocity = currentToTarget  * Time.fixedDeltaTime / movementDuration;
            velocity *= Mathf.Rad2Deg;
            var velocityVector = new Vector3(0, 0 ,velocity);

            var timer = 0.0f;
            while (timer < movementDuration) 
            {
                transform.eulerAngles += velocityVector ;
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
                StopRotation();
                rotationMovement = StartCoroutine(RotateUniformly(target, rotationDuration));
            }
        }

        public void StopRotation()
        {
            if (rotationMovement != null)
            {
                StopCoroutine(rotationMovement);
            }
        }
    }
}