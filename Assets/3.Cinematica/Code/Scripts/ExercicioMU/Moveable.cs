using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _3.Cinematica
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] private float translationDuration;
        [SerializeField] private bool useAcceleration;
        [SerializeField] private float rotationDuration;

        private Coroutine translationMovement;
        private Coroutine rotationMovement;
        
        public UnityEvent<Vector3> onTranslationCompleted;
        public UnityEvent<Vector3> onRotationCompleted;
        
        public IEnumerator LinearlyMoveTowards(Vector3 target, float movementDuration)
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
        
        
        public IEnumerator AccelerateTowards(Vector3 target, float movementDuration)
        {
            var currentToTarget = (target - transform.position);
            var targetVelocity = currentToTarget  * Time.fixedDeltaTime / movementDuration;
            var acceleration = targetVelocity / (movementDuration * movementDuration);
            var currentVelocity = Vector3.zero;

            var timer = 0.0f;
            var isStopping = false;
            while (timer < movementDuration)
            {
                if (isStopping == false && timer > movementDuration / 2)
                {
                    acceleration *= -1;
                    isStopping = true;
                }
                currentVelocity += acceleration * Time.fixedDeltaTime;
                transform.position += currentVelocity  * (movementDuration*4);
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            onTranslationCompleted.Invoke(target);
        }

        public void BeginNewTranslation(Vector3 target)
        {
            if (translationMovement == null)
            {
                translationMovement = StartCoroutine(useAcceleration 
                    ? AccelerateTowards(target, translationDuration) 
                    : LinearlyMoveTowards(target, translationDuration));
            }
            else
            {
                StopTranslation();
                translationMovement = StartCoroutine(useAcceleration 
                    ? AccelerateTowards(target, translationDuration) 
                    : LinearlyMoveTowards(target, translationDuration));
            }
        }

        public void StopTranslation()
        {
            if (translationMovement != null)
            {
                StopCoroutine(translationMovement);
            }
        } 
        
        public IEnumerator LinearlyRotateTowards(Vector3 target, float movementDuration)
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
        
        //TODO(Marlus) Create an accelerated version of rotation uniform motion
        
        public void BeginNewRotation(Vector3 target)
        {
            if (rotationMovement == null)
            {
                rotationMovement = StartCoroutine(LinearlyRotateTowards(target, rotationDuration));
            }
            else
            {
                StopRotation();
                rotationMovement = StartCoroutine(LinearlyRotateTowards(target, rotationDuration));
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